using System;
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
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="record"/> is <c>null</c>.
        /// </exception>
        public World(IDataRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");

            this.WorldId = (byte) record["WorldId"];
            this.WorldName = (string) record["WorldName"];
            this.ChannelCount = (byte) record["ChannelCount"];
        }
    }
}