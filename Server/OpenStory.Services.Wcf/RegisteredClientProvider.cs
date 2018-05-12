using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Wcf
{
    /// <summary>
    /// Provides client instances for a duplex service.
    /// </summary>
    /// <typeparam name="TChannel">The type of the service.</typeparam>
    public class RegisteredClientProvider<TChannel> : ServiceClientProvider<TChannel>
        where TChannel : class, IRegisteredService
    {
        private readonly DuplexChannelFactory<TChannel> _channelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredClientProvider{TChannel}"/> class.
        /// </summary>
        public RegisteredClientProvider()
        {
            _channelFactory = new DuplexChannelFactory<TChannel>(typeof(IServiceStateChanged), new NetTcpBinding(SecurityMode.Transport));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisteredClientProvider{TChannel}"/> class.
        /// </summary>
        /// <param name="handler">The object to use for service callbacks.</param>
        public RegisteredClientProvider(IServiceStateChanged handler)
        {
            _channelFactory = new DuplexChannelFactory<TChannel>(handler, new NetTcpBinding(SecurityMode.Transport));
        }

        /// <summary>
        /// Gets a service channel using discovery.
        /// </summary>
        public override TChannel CreateChannel()
        {
            _channelFactory.Open();
            var channel = _channelFactory.CreateChannel(Metadata.Address);
            return channel;
        }
    }
}
