using System.Net;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a configuration for a channel server.
    /// </summary>
    public sealed class ChannelConfiguration : ServerConfiguration
    {
        /// <summary>
        /// Gets the configured world identifier.
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Get sthe configured channel identifier.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ChannelConfiguration"/>.
        /// </summary>
        /// <param name="endpoint"><inheritdoc /></param>
        /// <param name="worldId">The world identifier for the server.</param>
        /// <param name="channelId">The channel identifier for the server.</param>
        public ChannelConfiguration(IPEndPoint endpoint, int worldId, int channelId)
            : base(endpoint)
        {
            this.WorldId = worldId;
            this.ChannelId = channelId;
        }
    }
}