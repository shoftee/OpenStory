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
    public sealed class WcfBootstrapper : BootstrapperBase, IDisposable
    {
        private bool isDisposed;
        
        private readonly List<OsWcfConfiguration> configurations;
        
        private readonly List<ServiceHost> hosts;

        /// <summary>
        /// Initializes it all.
        /// </summary>
        public WcfBootstrapper(IEnumerable<OsWcfConfiguration> configurations, IResolutionRoot resolutionRoot, ILogger logger)
            : base(resolutionRoot, logger)
        {
            this.configurations = configurations.ToList();
            this.hosts = new List<ServiceHost>(this.configurations.Count);

            this.isDisposed = false;
        }

        /// <inheritdoc/>
        protected override void OnStarting()
        {
            var sw = new Stopwatch();
            foreach (var configuration in this.configurations)
            {
                sw.Restart();

                var host = configuration.CreateHost(this.ResolutionRoot);
                this.hosts.Add(host);

                var serviceName = host.Description.Name ?? host.Description.ServiceType.FullName;
                host.Open();

                this.Logger.Debug("'{0}' started ({1} ms)", serviceName, sw.ElapsedMilliseconds);
            }

            sw.Stop();
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
