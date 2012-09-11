using OpenStory.Common.IO;

namespace OpenStory.Server.Channel
{
    internal class ChannelClient : ClientBase
    {
        public ChannelClient(ServerSession session) : base(session)
        {
        }

        protected override void ProcessPacket(ushort opCode, PacketReader reader)
        {
            // TODO packet handling
        }
    }
}
