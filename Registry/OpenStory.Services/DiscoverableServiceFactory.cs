using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Represents a base class for <see cref="IGenericServiceFactory"/> implementations which create discovery-enabled services.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    public abstract class DiscoverableServiceFactory<TService> : IGenericServiceFactory
    {
        private readonly Func<TService> serviceFactory;

        /// <summary>
        /// Gets the created service.
        /// </summary>
        protected TService Service { get; private set; }

        /// <summary>
        /// Gets the <see cref="ServiceConfiguration"/>.
        /// </summary>
        protected ServiceConfiguration Configuration { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoverableServiceFactory{TService}"/> class.
        /// </summary>
        /// <param name="serviceFactory">A delegate to use for creating the service objects.</param>
        public DiscoverableServiceFactory(Func<TService> serviceFactory)
        {
            this.serviceFactory = serviceFactory;
        }

        /// <inheritdoc/>
        public virtual ServiceHost CreateServiceHost()
        {
            var service = this.serviceFactory.Invoke();
            var host = new ServiceHost(service);

            this.Configuration = this.GetConfiguration();
            this.ConfigureServiceHost(host);

            this.Service = service;
            return host;
        }
        
        /// <summary>
        /// Configures the provided <see cref="ServiceHost"/> instance.
        /// </summary>
        /// <param name="serviceHost">The <see cref="ServiceHost"/> to configure.</param>
        protected virtual void ConfigureServiceHost(ServiceHost serviceHost)
        {
            this.AddDefaultEndpoint(serviceHost);
            this.AddDiscoveryBehavior(serviceHost);
            serviceHost.Closed += OnServiceHostClosed;
        }

        private void AddDiscoveryBehavior(ServiceHost host)
        {
            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
        }

        private void AddDefaultEndpoint(ServiceHost host)
        {
            var uri = this.Configuration.Get<Uri>(ServiceSettings.Uri.Key);
            var binding = new NetTcpBinding(SecurityMode.Transport);
            host.AddServiceEndpoint(typeof(TService), binding, uri);
        }

        private void OnServiceHostClosed(object sender, EventArgs eventArgs)
        {
            var disposable = this.Service as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }

            this.Service = default(TService);
        }

        /// <summary>
        /// Retrieves the <see cref="Configuration"/>.
        /// </summary>
        protected abstract ServiceConfiguration GetConfiguration();
    }
}