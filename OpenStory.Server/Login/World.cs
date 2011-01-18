using System.Collections.Generic;

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

    class World : IWorld
    {
        private List<Channel> channels;

        public World(int id, string name, int channelCount)
        {
            this.Id = id;
            this.Name = name;
            this.ChannelCount = channelCount;
            this.channels = new List<Channel>(channelCount);
        }

        #region IWorld Members

        public int Id { get; private set; }
        public string Name { get; private set; }
        public WorldStatus Status { get; set; }
        public int ChannelCount { get; private set; }

        public IEnumerable<IChannel> Channels
        {
            get { return this.channels; }
        }

        #endregion
    }
}