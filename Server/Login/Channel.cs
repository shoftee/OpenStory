using OpenMaple.Threading;

namespace OpenMaple.Server.Login
{
    class Channel : IChannel
    {
        public byte Id { get; private set; }
        public byte WorldId { get; private set; }
        public string Name { get; private set; }

        public int ChannelLoad { get { return channelLoad.Value; } }

        private AtomicInteger channelLoad;

        public Channel()
        {
            this.channelLoad = 0;
        }
    }
}
