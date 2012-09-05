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
    internal sealed class ActiveWorld : IWorld
    {
        private readonly List<ActiveChannel> channels;

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

        /// <summary>
        /// Initializes a new instance of <see cref="ActiveWorld"/>.
        /// </summary>
        public ActiveWorld(WorldInfo worldInfo)
        {
            this.Id = worldInfo.WorldId;
            this.Name = worldInfo.WorldName;
            this.ChannelCount = worldInfo.ChannelCount;
            this.channels = new List<ActiveChannel>(this.ChannelCount);
        }
    }
}
