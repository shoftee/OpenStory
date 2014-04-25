using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Server.Processing;

namespace OpenStory.Server.Auth
{
    internal static class PacketHelpers
    {
        public static byte[] PinResponse(this IPacketFactory packetFactory, PinResponseType result)
        {
            using (var builder = packetFactory.CreatePacket("PinResponse"))
            {
                builder.WriteByte(result);

                return builder.ToByteArray();
            }
        }
    }
}
