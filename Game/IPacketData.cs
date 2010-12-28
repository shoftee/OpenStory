
using OpenMaple.IO;

namespace OpenMaple.Game
{
    interface IPacketData
    {
        void WriteData(PacketWriter writer);
    }
}