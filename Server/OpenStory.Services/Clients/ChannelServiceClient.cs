using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to the game channel service.
    /// </summary>
    public sealed class ChannelServiceClient : GameServiceClient<IChannelService>
    {
        /// <summary>
        /// Initialized a new instance of <see cref="ChannelServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        public ChannelServiceClient(Uri uri)
            : base(uri)
        {
        }
    }
}
