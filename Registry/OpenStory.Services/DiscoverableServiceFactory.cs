using System;
using System.ServiceModel;
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
        /// <summary>
        /// Gets the created service.
        /// </summary>
        protected TService Service { get; private set; }

        /// <summary>
        /// Creates a <typeparamref name="TService"/> instance.
        /// </summary>
        protected abstract TService CreateService();

        /// <summary>
        /// Configures the provided <see cref="ServiceHost"/> instance.
        /// </summary>
        /// <param name="serviceHost">The <see cref="ServiceHost"/> to configure.</param>
        protected virtual void ConfigureServiceHost(ServiceHost serviceHost)
        {
            serviceHost.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            serviceHost.AddServiceEndpoint(new UdpDiscoveryEndpoint());
        }

        /// <inheritdoc/>
        public virtual ServiceHost CreateServiceHost()
        {
            var service = this.CreateService();

            var host = new ServiceHost(service);
            this.ConfigureServiceHost(host);

            this.Service = service;
            return host;
        }
    }
}