using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using Ninject.Extensions.Logging;
using OpenStory.Services;
using OpenStory.Services.Registry;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Represents a default implementation of the <see cref="IServiceConfigurationProvider"/> interface.
    /// </summary>
    public class DefaultServiceConfigurationProvider : IServiceConfigurationProvider
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServiceConfigurationProvider"/> class.
        /// </summary>
        public DefaultServiceConfigurationProvider(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public ServiceConfiguration GetConfiguration(NexusConnectionInfo nexusConnectionInfo)
        {
            logger.Debug("Discovering registry service...");
            var endpoint = this.GetRegistry();
            logger.Debug("Resolved registry at '{0}'.", endpoint.Address);

            using (var registry = new RegistryServiceClient(endpoint))
            {
                var response = registry.GetServiceConfiguration(nexusConnectionInfo.AccessToken);
                var configuration = response.GetResult();
                return configuration;
            }
        }

        private ServiceEndpoint GetRegistry()
        {
            var client = new DiscoveryClient(new UdpDiscoveryEndpoint());
            var criteria = new FindCriteria(typeof(IRegistryService))
                           {
                               MaxResults = 1,
                               Duration = TimeSpan.FromSeconds(5),
                           };

            var response = client.Find(criteria);
            var endpointMetadata = response.Endpoints.First();

            var contract = ContractDescription.GetContract(typeof(IRegistryService));
            var endpoint = new ServiceEndpoint(contract, new NetTcpBinding(SecurityMode.Transport), endpointMetadata.Address);
            return endpoint;
        }
    }
}
