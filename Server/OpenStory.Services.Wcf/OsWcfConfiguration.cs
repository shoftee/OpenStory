using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using Ninject;
using Ninject.Infrastructure.Language;
using Ninject.Syntax;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Represents a configuration for a WCF service.
    /// </summary>
    public class OsWcfConfiguration
    {
        /// <summary>
        /// Gets the channel type of the service.
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the list of base addresses for the service.
        /// </summary>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// Gets the configuration callback for the service.
        /// </summary>
        public Action<ServiceHost> ApplyTo { get; private set; }

        private OsWcfConfiguration(Type serviceType, Uri baseUri, Action<ServiceHost> applyTo)
        {
            this.ServiceType = serviceType;
            this.BaseUri = baseUri;
            this.ApplyTo = applyTo ?? (host => { });
        }

        /// <summary>
        /// Creates the service host from this configuration.
        /// </summary>
        public ServiceHost CreateHost(IResolutionRoot resolutionRoot)
        {
            var service = resolutionRoot.Get(this.ServiceType);
            var host = new ServiceHost(service, this.BaseUri);
            this.ApplyTo(host);

            var description = host.Description;
            if (!(service is IRegisteredService))
            {
                description.Behaviors.Add(new ServiceDiscoveryBehavior());
                description.Endpoints.Add(new UdpDiscoveryEndpoint());
            }

            var binding = new NetTcpBinding(SecurityMode.Transport);
            var interfaces = GetPossibleContracts(this.ServiceType);
            foreach (var @interface in interfaces)
            {
                var attribute = GetContractAttribute(@interface);
                if (attribute != null)
                {
                    host.AddServiceEndpoint(@interface, binding, attribute.Name);
                }
            }

            return host;
        }

        private static ServiceContractAttribute GetContractAttribute(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(ServiceContractAttribute), false);
            var attribute = attributes.OfType<ServiceContractAttribute>().FirstOrDefault();
            return attribute;
        }

        private static IEnumerable<Type> GetPossibleContracts(Type serviceType)
        {
            return serviceType
                .GetAllBaseTypes()
                .SelectMany(t => t.GetInterfaces())
                .Distinct();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OsWcfConfiguration"/> class.
        /// </summary>
        public static OsWcfConfiguration For<TService>(Uri baseUri, Action<ServiceHost> applyConfiguration = null)
            where TService : class
        {
            var serviceType = typeof(TService);

            if (!serviceType.IsClass)
            {
                throw new ArgumentException("The provided service type must be a class.");
            }

            return new OsWcfConfiguration(typeof(TService), baseUri, applyConfiguration);
        }
    }
}