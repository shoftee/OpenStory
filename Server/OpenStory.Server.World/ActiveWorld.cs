using System.Collections.Generic;
using System.Runtime.Serialization;
using OpenStory.Common.Game;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.World
{
    /// <summary>
    /// Represents a game World.
    /// </summary>
    [DataContract]
    internal sealed class ActiveWorld : IWorld
    {
        [DataMember]
        private readonly List<ActiveChannel> channels;

        #region IWorld Members

        /// <summary>
        /// Gets the internal ID of the World.
        /// </summary>
        [DataMember]
        public int Id { get; private set; }

        /// <summary>
        /// Gets the name of the World.
        /// </summary>
        [DataMember]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the number of channels in the World.
        /// </summary>
        [DataMember]
        public int ChannelCount { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ServerStatus"/> for the World.
        /// </summary>
        [DataMember]
        public ServerStatus Status { get; set; }

        /// <summary>
        /// Gets an enumerable list of channels for the World.
        /// </summary>
        IEnumerable<IChannel> IWorld.Channels => this.channels.AsReadOnly();

        #endregion

        /// <summary>
        /// Gets the list of active channels.
        /// </summary>
        public List<ActiveChannel> Channels => this.channels;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveWorld"/> class.
        /// </summary>
        public ActiveWorld(WorldInfo worldInfo)
        {
            this.Id = worldInfo.WorldId;
            this.Name = worldInfo.WorldName;
            this.ChannelCount = worldInfo.ChannelCount;
            this.channels = new List<ActiveChannel>();
        }
    }
}
