using Ninject.Extensions.Logging;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a network client for a channel server.
    /// </summary>
    public sealed class ChannelClient : ClientBase
    {
        /// <inheritdoc/>
        public ChannelClient(IServerSession session, IPacketFactory packetFactory, ILogger logger)
            : base(session, packetFactory, logger)
        {
        }

        /// <inheritdoc/>
        protected override void ProcessPacket(PacketProcessingEventArgs args)
        {
            // TODO: Channel packet handling, hello?
        }
    }
}
