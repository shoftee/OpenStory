using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Provides client instances for a service.
    /// </summary>
    /// <typeparam name="TChannel">The type of the service.</typeparam>
    public class ServiceClientProvider<TChannel> : IServiceClientProvider<TChannel>
        where TChannel : class
    {
        private readonly ChannelFactory<TChannel> _channelFactory;
        private readonly Lazy<EndpointDiscoveryMetadata> _metadata;

        /// <summary>
        /// Gets the discovered metadata information.
        /// </summary>
        protected EndpointDiscoveryMetadata Metadata => _metadata.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientProvider{TContract}"/> class.
        /// </summary>
        public ServiceClientProvider()
        {
            _channelFactory = new ChannelFactory<TChannel>(new NetTcpBinding(SecurityMode.Transport));
            _metadata = new Lazy<EndpointDiscoveryMetadata>(GetEndpointMetadata);
        }

        /// <summary>
        /// Gets a service channel using discovery.
        /// </summary>
        public virtual TChannel CreateChannel()
        {
            if (_channelFactory.State == CommunicationState.Created)
            {
                _channelFactory.Open();
            }

            var channel = _channelFactory.CreateChannel(Metadata.Address);
            return channel;
        }

        private static EndpointDiscoveryMetadata GetEndpointMetadata()
        {
            var discoveryClient = GetDiscoveryClient();
            var findCriteria = GetFindCriteria();
            var response = discoveryClient.Find(findCriteria);
            var metadata = response.Endpoints.FirstOrDefault();
            if (metadata == null)
            {
                var message = $"No endpoint found for contract '{typeof(TChannel).FullName}'.";
                throw new InvalidOperationException(message);
            }

            return metadata;
        }

        private static DiscoveryClient GetDiscoveryClient()
        {
            var discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
            return discoveryClient;
        }

        private static FindCriteria GetFindCriteria()
        {
            return new FindCriteria(typeof(TChannel))
            {
                Duration = TimeSpan.FromSeconds(5),
                MaxResults = 1,
            };
        }
    }
}
