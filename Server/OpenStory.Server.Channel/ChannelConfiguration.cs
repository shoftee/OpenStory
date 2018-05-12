using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a configuration for a channel server.
    /// </summary>
    public sealed class ChannelConfiguration : ServerConfiguration
    {
        /// <summary>
        /// Gets the configured channel identifier.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelConfiguration"/> class.
        /// </summary>
        /// <param name="configuration"><inheritdoc /></param>
        public ChannelConfiguration(OsServiceConfiguration configuration)
            : base(configuration)
        {
            ChannelId = configuration.Get<int>("Channel");
        }
    }
}