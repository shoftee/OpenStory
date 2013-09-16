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
        private readonly ChannelFactory<TChannel> channelFactory;
        private readonly Lazy<EndpointDiscoveryMetadata> metadata;

        /// <summary>
        /// Gets the discovered metadata information.
        /// </summary>
        protected EndpointDiscoveryMetadata Metadata
        {
            get { return this.metadata.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientProvider{TContract}"/> class.
        /// </summary>
        public ServiceClientProvider()
        {
            this.channelFactory = new ChannelFactory<TChannel>(new NetTcpBinding(SecurityMode.Transport));
            this.metadata = new Lazy<EndpointDiscoveryMetadata>(GetEndpointMetadata);
        }

        /// <summary>
        /// Gets a service channel using discovery.
        /// </summary>
        public virtual TChannel CreateChannel()
        {
            if (this.channelFactory.State == CommunicationState.Created)
            {
                this.channelFactory.Open();
            }

            var channel = this.channelFactory.CreateChannel(this.Metadata.Address);
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
                var message = string.Format("No endpoint found for contract '{0}'.", typeof(TChannel).FullName);
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
