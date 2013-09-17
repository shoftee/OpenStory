using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Ninject.Extensions.Logging;
using Ninject.Syntax;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// It's a bootstrapper. Yeah.
    /// </summary>
    public class WcfBootstrapper : IDisposable, IBootstrapper
    {
        private bool isDisposed;

        /// <summary>
        /// Gets the resolution root for the bootstrapper.
        /// </summary>
        protected IResolutionRoot ResolutionRoot { get; private set; }

        /// <summary>
        /// Gets the logger for this bootstrapper.
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Gets the list of configurations for the bootstrapper.
        /// </summary>
        protected List<OsWcfConfiguration> Configurations { get; private set; }

        private readonly List<ServiceHost> hosts;
        
        /// <summary>
        /// Initializes it all.
        /// </summary>
        public WcfBootstrapper(
            IResolutionRoot resolutionRoot, 
            IEnumerable<OsWcfConfiguration> configurations,
            ILogger logger)
        {
            this.ResolutionRoot = resolutionRoot;
            this.Configurations = configurations.ToList();
            this.Logger = logger;

            this.hosts = new List<ServiceHost>(this.Configurations.Count);
        }

        /// <inheritdoc/>
        public void Start()
        {
            try
            {
                this.Logger.Info("Starting services...");
                var sw = new Stopwatch();
                foreach (var configuration in this.Configurations)
                {
                    sw.Restart();

                    var host = configuration.CreateHost(this.ResolutionRoot);
                    this.hosts.Add(host);

                    var serviceName = host.Description.Name ?? host.Description.ServiceType.FullName;
                    host.Open();

                    this.Logger.Debug("'{0}' started ({1} ms)", serviceName, sw.ElapsedMilliseconds);
                }

                sw.Stop();

                this.Logger.Info("All services started.");
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex, null);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                foreach (var host in this.hosts)
                {
                    if (host.State != CommunicationState.Faulted)
                    {
                        host.Close();
                    }
                    else
                    {
                        host.Abort();
                    }
                }
                
                this.hosts.Clear();

                this.isDisposed = true;
            }
        }
    }
}
