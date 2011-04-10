using System.Data;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// An object mapping for the World table.
    /// </summary>
    public partial class World
    {
        /// <summary>
        /// Gets the world ID.
        /// </summary>
        public byte WorldId { get; private set; }

        /// <summary>
        /// Gets the world name.
        /// </summary>
        public string WorldName { get; private set; }

        /// <summary>
        /// Gets the number of channels in the world.
        /// </summary>
        public byte ChannelCount { get; private set; }

        /// <summary>
        /// Initializes this World object from a given <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="record">The record containing the data for this object.</param>
        public World(IDataRecord record)
        {
            this.WorldId = (byte) record["WorldId"];
            this.WorldName = (string) record["WorldName"];
            this.ChannelCount = (byte) record["ChannelCount"];
        }
    }
}
