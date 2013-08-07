using System.Net;
using OpenStory.Framework.Contracts;

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
        /// Gets the configured channel identifier.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <inheritdoc />
        public ChannelConfiguration(ServiceConfiguration configuration)
            : base(configuration)
        {
            this.WorldId = configuration.Get<int>("World");
            this.ChannelId = configuration.Get<int>("Channel");
        }
    }
}