using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
        private readonly ServiceClientProvider<IRegistryService> provider;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServiceConfigurationProvider"/> class.
        /// </summary>
        public DefaultServiceConfigurationProvider(ServiceClientProvider<IRegistryService> provider, ILogger logger)
        {
            this.provider = provider;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public ServiceConfiguration GetConfiguration(NexusConnectionInfo nexusConnectionInfo)
        {
            var client = this.provider.GetClient();

            using ((IClientChannel)client)
            {
                var configuration = client.GetServiceConfiguration(nexusConnectionInfo.AccessToken);
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
