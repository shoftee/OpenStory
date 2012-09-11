using OpenStory.Common.IO;

namespace OpenStory.Server.Channel
{
    internal sealed class ChannelClient : ClientBase
    {
        private readonly IChannelServer server;

        public ChannelClient(IChannelServer server, ServerSession session)
            : base(server, session)
        {
            this.server = server;
        }

        protected override void ProcessPacket(string label, PacketReader reader)
        {
            // TODO packet handling
        }
    }
}
