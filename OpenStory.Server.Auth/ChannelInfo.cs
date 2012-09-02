using OpenStory.Common;
using OpenStory.Common.Auth;

namespace OpenStory.Server.Auth
{
    internal class ChannelInfo : IChannel
    {
        private readonly AtomicInteger channelLoad;

        public byte Id { get; private set; }
        public byte WorldId { get; private set; }
        public string Name { get; private set; }

        public int ChannelLoad
        {
            get { return this.channelLoad.Value; }
        }

        public ChannelInfo()
        {
            this.channelLoad = 0;
        }
    }
}
