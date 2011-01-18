using OpenStory.Common.Threading;

namespace OpenStory.Server.Login
{
    /// <summary>
    /// Provides properties for game channels.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Gets the ID number of the channel.
        /// </summary>
        byte Id { get; }

        /// <summary>
        /// Gets the ID of the world hosting the channel.
        /// </summary>
        byte WorldId { get; }

        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a number between 0 and 1200 denoting how populated the channel is.
        /// </summary>
        int ChannelLoad { get; }
    }

    class Channel : IChannel
    {
        private AtomicInteger channelLoad;

        public Channel()
        {
            this.channelLoad = 0;
        }

        #region IChannel Members

        public byte Id { get; private set; }
        public byte WorldId { get; private set; }
        public string Name { get; private set; }

        public int ChannelLoad
        {
            get { return this.channelLoad.Value; }
        }

        #endregion
    }
}