using OpenStory.Common.Threading;

namespace OpenStory.Server.Login
{
    internal class Channel : IChannel
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