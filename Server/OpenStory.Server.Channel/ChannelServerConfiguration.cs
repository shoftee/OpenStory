using System.Net;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a configuration for a channel server.
    /// </summary>
    public sealed class ChannelServerConfiguration : ServerConfiguration
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
        /// Initializes a new instance of <see cref="ChannelServerConfiguration"/>.
        /// </summary>
        /// <param name="address"><inheritdoc /></param>
        /// <param name="port"><inheritdoc /></param>
        /// <param name="worldId">The world identifier for the server.</param>
        /// <param name="channelId">The channel identifier for the server.</param>
        public ChannelServerConfiguration(IPAddress address, int port, int worldId, int channelId)
            : base(address, port)
        {
            this.WorldId = worldId;
            this.ChannelId = channelId;
        }
    }
}