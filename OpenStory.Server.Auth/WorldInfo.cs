using System.Collections.Generic;
using OpenStory.Common.Auth;
using OpenStory.Common.Tools;
using OpenStory.Server.Auth.Data;
using OpenStory.Server.Data;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents a game World.
    /// </summary>
    internal class WorldInfo : IWorld
    {
        private readonly List<ChannelInfo> channels;

        /// <summary>
        /// Initializes a new instance of <see cref="WorldInfo"/>.
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
        /// Gets the <see cref="ServerStatus"/> for the World.
        /// </summary>
        public ServerStatus Status { get; set; }

        /// <summary>
        /// Gets the number of channels in the World.
        /// </summary>
        public int ChannelCount { get; private set; }

        /// <summary>
        /// Gets an enumerable list of channels for the World.
        /// </summary>
        public IEnumerable<IChannel> Channels
        {
            get { return this.channels.ToReadOnly(); }
        }

        #endregion
    }
}