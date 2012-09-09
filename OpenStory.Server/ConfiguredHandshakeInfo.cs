using OpenStory.Common.IO;

namespace OpenStory.Server
{
    internal sealed class ConfiguredHandshakeInfo : HandshakeInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HandshakeInfo"/> with some predefined values.
        /// </summary>
        /// <param name="version">The game version.</param>
        /// <param name="subversion">The game sub-version.</param>
        /// <param name="clientIv">The client cryptographic IV.</param>
        /// <param name="serverIv">The server cryptographic IV.</param>
        public ConfiguredHandshakeInfo(ushort version, string subversion, byte[] clientIv, byte[] serverIv)
        {
            this.Header = 0x000E; // TODO: load configured setting for this?
            this.Version = version;
            this.SubVersion = subversion;
            this.ClientIv = clientIv;
            this.ServerIv = serverIv;
            this.ServerId = 0x05; // TODO: load configured setting for this?
        }
    }
}
