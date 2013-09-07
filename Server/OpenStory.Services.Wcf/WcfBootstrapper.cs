using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Ninject.Syntax;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// It's a bootstrapper. Yeah.
    /// </summary>
    public class WcfBootstrapper : IDisposable, IBootstrapper
    {
        private bool isDisposed;

        private readonly IResolutionRoot resolutionRoot;
        private readonly List<WcfConfiguration> configurations;
        private readonly List<ServiceHost> hosts;
        
        /// <summary>
        /// Initializes it all.
        /// </summary>
        public WcfBootstrapper(IResolutionRoot resolutionRoot, IEnumerable<WcfConfiguration> configurations)
        {
            this.resolutionRoot = resolutionRoot;
            this.configurations = configurations.ToList();

            this.hosts = new List<ServiceHost>(this.configurations.Count);
        }

        /// <inheritdoc/>
        public void Start()
        {
            foreach (var configuration in this.configurations)
            {
                var baseAddresses = configuration.BaseAddresses.ToArray();
                var host = new ServiceHost(configuration.ServiceType, baseAddresses);
                this.hosts.Add(host);

                host.Open();
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
