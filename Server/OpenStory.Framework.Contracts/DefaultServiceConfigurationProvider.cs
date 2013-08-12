using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;
using OpenStory.Services;
using OpenStory.Services.Registry;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Represents a default implementation of the <see cref="IServiceConfigurationProvider"/> interface.
    /// </summary>
    public class DefaultServiceConfigurationProvider : IServiceConfigurationProvider
    {
        private readonly DiscoveryEndpointProvider discoveryEndpointProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServiceConfigurationProvider"/> class.
        /// </summary>
        public DefaultServiceConfigurationProvider(DiscoveryEndpointProvider discoveryEndpointProvider)
        {
            this.discoveryEndpointProvider = discoveryEndpointProvider;
        }

        /// <inheritdoc/>
        public ServiceConfiguration GetConfiguration(NexusConnectionInfo nexusConnectionInfo)
        {
            var criteria = GetDefaultFindCriteria<IRegistryService>();
            var endpoint = GetEndpoint<IRegistryService>(criteria);

            using (var registry = new RegistryServiceClient(endpoint))
            {
                var configuration = registry.GetServiceConfiguration(nexusConnectionInfo.AccessToken).GetResult();
                return configuration;
            }
        }

        private ServiceEndpoint GetEndpoint<TContract>(FindCriteria criteria)
        {
            var discoveryEndpoint = this.discoveryEndpointProvider.GetDiscoveryEndpoint();
            var client = new DiscoveryClient(discoveryEndpoint);
            var endpointMetadata = client.Find(criteria).Endpoints.First();

            var contract = ContractDescription.GetContract(typeof(TContract));
            var endpoint = new ServiceEndpoint(contract, new NetTcpBinding(SecurityMode.Transport), endpointMetadata.Address);
            return endpoint;
        }

        private static FindCriteria GetDefaultFindCriteria<TContract>()
        {
            var criteria = new FindCriteria(typeof(TContract))
                           {
                               Duration = TimeSpan.FromSeconds(30),
                           };

            criteria.Scopes.Add(new Uri("net.tcp://OpenStory/"));
            criteria.ScopeMatchBy = FindCriteria.ScopeMatchByPrefix;

            return criteria;
        }
    }
}
