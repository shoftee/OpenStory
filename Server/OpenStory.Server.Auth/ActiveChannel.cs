using OpenStory.Common;
using OpenStory.Common.Game;

namespace OpenStory.Server.Auth
{
    internal sealed class ActiveChannel : IChannel
    {
        private readonly AtomicInteger channelLoad;

        public byte ChannelId { get; private set; }

        public byte WorldId { get; private set; }

        public string Name { get; private set; }

        public int ChannelLoad
        {
            get { return this.channelLoad.Value; }
        }

        public ActiveChannel()
        {
            this.channelLoad = 0;
        }
    }
}
