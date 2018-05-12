using OpenStory.Common;
using OpenStory.Common.IO;

namespace OpenStory.Server.Processing
{
    internal sealed class PacketFactory : IPacketFactory
    {
        private readonly IPacketCodeTable _packets;

        public PacketFactory(IPacketCodeTable packets)
        {
            _packets = packets;
        }

        public PacketBuilder CreatePacket(string label)
        {
            ushort code = _packets.GetOutgoingCode(label);

            var builder = new PacketBuilder();
            builder.WriteInt16(code);
            return builder;
        }
    }
}
