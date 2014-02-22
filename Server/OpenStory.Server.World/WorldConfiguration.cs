using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    /// <summary>
    /// Represents a configuration for a world server.
    /// </summary>
    public sealed class WorldConfiguration
    {
        /// <summary>
        /// Gets the configured world identifier.
        /// </summary>
        public int WorldId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldConfiguration"/> class.
        /// </summary>
        /// <param name="configuration"><inheritdoc /></param>
        public WorldConfiguration(OsServiceConfiguration configuration)
        {
            this.WorldId = configuration.Get<int>("World");
        }
    }
}
