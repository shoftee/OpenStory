using OpenStory.Common.IO;

namespace OpenStory.Redirector.Connection
{
    internal sealed class PackagedHandshakeInfo : HandshakeInfo
    {
        public bool TryParse(PacketReader reader)
        {
            return reader.Safe(this.ReadHandshake, () => false);
        }

        private bool ReadHandshake(IUnsafePacketReader reader)
        {
            ushort header = reader.ReadUInt16();
            ushort version = reader.ReadUInt16();
            string subversion = reader.ReadLengthString();

            if (subversion.Length == 0)
            {
                if (header != 0x0D)
                {
                    return false;
                }
            }
            else
            {
                if (header != 0x0E)
                {
                    return false;
                }
            }

            byte[] clientIv = reader.ReadBytes(4);
            byte[] serverIv = reader.ReadBytes(4);

            byte serverId = reader.ReadByte();

            this.Header = header;
            this.Version = version;
            this.Subversion = subversion;
            this.ClientIv = clientIv;
            this.ServerIv = serverIv;
            this.ServerId = serverId;

            return true;
        }
    }
}