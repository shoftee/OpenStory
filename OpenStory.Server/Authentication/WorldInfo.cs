using System.Collections.Generic;
using OpenStory.Common.Authentication;
using OpenStory.Server.Data;

namespace OpenStory.Server.Authentication
{
    /// <summary>
    /// Represents a game World.
    /// </summary>
    class WorldInfo : IWorld
    {
        private List<ChannelInfo> channels;

        /// <summary>
        /// Initializes a new instance of the WorldInfo class.
        /// </summary>
        public WorldInfo(World world)
        {
            this.Id = world.WorldId;
            this.Name = world.WorldName;
            this.ChannelCount = world.ChannelCount;
            this.channels = new List<ChannelInfo>(this.ChannelCount);
        }

        #region IWorld Members

        /// <summary>
        /// Gets the internal ID of the World.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name of the World.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the <see cref="WorldStatus"/> for the World.
        /// </summary>
        public WorldStatus Status { get; set; }

        /// <summary>
        /// Gets the number of channels in the World.
        /// </summary>
        public int ChannelCount { get; private set; }

        /// <summary>
        /// Gets an enumerable list of channels for the World.
        /// </summary>
        public IEnumerable<IChannel> Channels
        {
            get { return this.channels; }
        }

        #endregion
    }
}