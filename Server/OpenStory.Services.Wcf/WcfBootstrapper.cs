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
        private bool _isDisposed;
        
        private readonly List<OsWcfConfiguration> _configurations;
        
        private readonly List<ServiceHost> _hosts;

        /// <summary>
        /// Initializes it all.
        /// </summary>
        public WcfBootstrapper(IEnumerable<OsWcfConfiguration> configurations, IResolutionRoot resolutionRoot, ILogger logger)
            : base(resolutionRoot, logger)
        {
            _configurations = configurations.ToList();
            _hosts = new List<ServiceHost>(_configurations.Count);

            _isDisposed = false;
        }

        /// <inheritdoc/>
        protected override void OnStarting()
        {
            var sw = new Stopwatch();
            foreach (var configuration in _configurations)
            {
                sw.Restart();

                var host = configuration.CreateHost(ResolutionRoot);
                _hosts.Add(host);

                var serviceName = host.Description.Name ?? host.Description.ServiceType.FullName;
                host.Open();

                Logger.Debug("'{0}' started ({1} ms)", serviceName, sw.ElapsedMilliseconds);
            }

            sw.Stop();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                foreach (var host in _hosts)
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

                _hosts.Clear();

                _isDisposed = true;
            }
        }
    }
}
