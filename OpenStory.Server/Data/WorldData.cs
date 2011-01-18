using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// An object mapping for the [World] table.
    /// </summary>
    public class WorldData
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

        internal WorldData(byte worldId, string worldName, byte channelCount)
        {
            this.WorldId = worldId;
            this.WorldName = worldName;
            this.ChannelCount = channelCount;
        }
    }
}
