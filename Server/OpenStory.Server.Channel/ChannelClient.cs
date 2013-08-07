using Ninject.Extensions.Logging;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;

namespace OpenStory.Server.Channel
{
    internal sealed class ChannelClient : ClientBase
    {
        public ChannelClient(IServerSession session, IPacketFactory packetFactory, ILogger logger)
            : base(session, packetFactory, logger)
        {
        }

        protected override void ProcessPacket(PacketProcessingEventArgs args)
        {
            // TODO packet handling
        }
    }
}
