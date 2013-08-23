using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;

namespace OpenStory.Services
{
    /// <summary>
    /// Provides client instances for a service.
    /// </summary>
    /// <typeparam name="TChannel">The type of the service.</typeparam>
    public class ServiceClientProvider<TChannel>
        where TChannel : class
    {
        private readonly ChannelFactory<TChannel> channelFactory;
        private readonly Lazy<EndpointDiscoveryMetadata> metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientProvider{TContract}"/> class.
        /// </summary>
        public ServiceClientProvider()
        {
            this.channelFactory = new ChannelFactory<TChannel>(new NetTcpBinding(SecurityMode.Transport));
            this.metadata = new Lazy<EndpointDiscoveryMetadata>(GetEndpointMetadata);
        }

        private static FindCriteria GetFindCriteria()
        {
            return new FindCriteria(typeof(TChannel))
            {
                Duration = TimeSpan.FromSeconds(5),
                MaxResults = 1,
            };
        }

        /// <summary>
        /// Gets a service channel using discovery.
        /// </summary>
        public TChannel GetClient()
        {
            return CreateChannel(metadata.Value.Address);
        }

        private static EndpointDiscoveryMetadata GetEndpointMetadata()
        {
            var discoveryClient = GetDiscoveryClient();
            var findCriteria = GetFindCriteria();
            var response = discoveryClient.Find(findCriteria);
            var metadata = response.Endpoints.FirstOrDefault();
            if (metadata == null)
            {
                var message = string.Format("No endpoint found for contract {0}", typeof(TChannel).FullName);
                throw new InvalidOperationException(message);
            }

            return metadata;
        }

        private static DiscoveryClient GetDiscoveryClient()
        {
            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            return discoveryClient;
        }

        /// <summary>
        /// Gets a service channel to the specified address.
        /// </summary>
        /// <param name="uri">The address of the service.</param>
        public TChannel GetClient(string uri)
        {
            return CreateChannel(new EndpointAddress(uri));         
        }

        private TChannel CreateChannel(EndpointAddress address)
        {
            channelFactory.Open();
            var channel = channelFactory.CreateChannel(address);
            return channel;
        }
    }
}
