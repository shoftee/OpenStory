using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Common.Game;
using OpenStory.Framework.Model.Auth;

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
        /// Gets or sets the <see cref="ServerStatus"/> for the World.
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
            get { return this.channels.AsReadOnly(); }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveWorld"/> class.
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
