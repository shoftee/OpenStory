using System.Collections.Generic;
using OpenStory.Server.Data;

namespace OpenStory.Server.Login
{
    /// <summary>
    /// Provides properties of a game World.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// Gets the internal ID of the World.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the name of the World.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="WorldStatus"/> for the World.
        /// </summary>
        WorldStatus Status { get; }

        /// <summary>
        /// Gets the number of channels in the World.
        /// </summary>
        int ChannelCount { get; }

        /// <summary>
        /// Gets an enumerable list of channels for the World.
        /// </summary>
        IEnumerable<IChannel> Channels { get; }
    }

    /// <summary>
    /// Represents a game World.
    /// </summary>
    public class World : IWorld
    {
        private List<Channel> channels;

        /// <summary>
        /// Initializes a new instance of the World class.
        /// </summary>
        public World(WorldData worldData)
        {
            this.Id = worldData.WorldId;
            this.Name = worldData.WorldName;
            this.ChannelCount = worldData.ChannelCount;
            this.channels = new List<Channel>(this.ChannelCount);
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