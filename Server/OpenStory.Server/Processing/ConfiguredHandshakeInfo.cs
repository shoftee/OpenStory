using OpenStory.Common.IO;
using OpenStory.Server.Fluent;

namespace OpenStory.Server.Processing
{
    internal sealed class ConfiguredHandshakeInfo : HandshakeInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="HandshakeInfo"/> with some predefined values.
        /// </summary>
        /// <param name="header">The header of the handshake packet.</param>
        /// <param name="version">The game version.</param>
        /// <param name="subversion">The game sub-version.</param>
        /// <param name="clientIv">The client cryptographic IV.</param>
        /// <param name="serverIv">The server cryptographic IV.</param>
        /// <param name="serverId">The server identifier.</param>
        public ConfiguredHandshakeInfo(ushort header, ushort version, string subversion, byte[] clientIv, byte[] serverIv, byte serverId)
        {
            this.Header = header;
            this.Version = version;
            this.Subversion = subversion;
            this.ClientIv = clientIv;
            this.ServerIv = serverIv;
            this.ServerId = serverId;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="HandshakeInfo"/> with the server-configured values.
        /// </summary>
        /// <param name="clientIv">The client cryptographic IV.</param>
        /// <param name="serverIv">The server cryptographic IV.</param>
        public ConfiguredHandshakeInfo(byte[] clientIv, byte[] serverIv)
        {
            var facade = OS.Config().Domain("Handshake");

            this.Header = facade.Get<ushort>("Header");
            this.Version = facade.Get<ushort>("Version");
            this.Subversion = facade.Get<string>("Subversion");
            this.ServerId = facade.Get<byte>("ServerId");

            this.ClientIv = clientIv;
            this.ServerIv = serverIv;
        }
    }
}
