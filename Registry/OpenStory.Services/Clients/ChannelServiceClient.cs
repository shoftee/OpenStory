using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to the game channel service.
    /// </summary>
    public sealed class ChannelServiceClient : GameServiceClientBase<IChannelService>, IChannelService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="ChannelServiceClient"/> with the specified endpoint.
        /// </summary>
        /// <inheritdoc />
        public ChannelServiceClient(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }
    }
}
