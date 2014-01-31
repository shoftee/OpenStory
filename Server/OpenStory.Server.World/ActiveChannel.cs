using System.Runtime.Serialization;
using OpenStory.Common;
using OpenStory.Common.Game;

namespace OpenStory.Server.World
{
    /// <summary>
    /// Contains basic information about an active game channel.
    /// </summary>
    [DataContract]
    internal sealed class ActiveChannel : IChannel
    {
        [DataMember]
        private readonly AtomicInteger channelLoad;

        #region IChannel Members

        /// <inheritdoc/>
        [DataMember]
        public byte ChannelId { get; private set; }

        /// <inheritdoc/>
        [DataMember]
        public byte WorldId { get; private set; }

        /// <inheritdoc/>
        [DataMember]
        public string Name { get; private set; }

        /// <inheritdoc/>
        int IChannel.ChannelLoad
        {
            get { return this.channelLoad.Value; }
        }

        #endregion

        /// <summary>
        /// Gets the channel load value holder.
        /// </summary>
        public AtomicInteger ChannelLoad
        {
            get { return this.channelLoad; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveChannel"/> class.
        /// </summary>
        public ActiveChannel()
        {
            this.channelLoad = 0;
        }
    }
}
