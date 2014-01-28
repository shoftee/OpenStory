namespace OpenStory.Framework.Model.Auth
{
    /// <summary>
    /// An object mapping for the World table.
    /// </summary>
    public sealed class WorldInfo
    {
        /// <summary>
        /// Gets the world ID.
        /// </summary>
        public byte WorldId { get; set; }

        /// <summary>
        /// Gets the world name.
        /// </summary>
        public string WorldName { get; set; }

        /// <summary>
        /// Gets the number of channels in the world.
        /// </summary>
        public int ChannelCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldInfo"/> class.
        /// </summary>
        public WorldInfo()
        {
        }
    }
}
