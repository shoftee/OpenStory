using OpenStory.Common.IO;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    internal sealed class ConfiguredHandshakeInfo : HandshakeInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfiguredHandshakeInfo"/> class.
        /// </summary>
        /// <param name="configuration">The server configuration.</param>
        /// <param name="clientIv">The client cryptographic IV.</param>
        /// <param name="serverIv">The server cryptographic IV.</param>
        public ConfiguredHandshakeInfo(ServerConfiguration configuration, byte[] clientIv, byte[] serverIv)
        {
            Header = configuration.Header;
            Version = configuration.Version;
            Subversion = configuration.Subversion;
            LocaleId = configuration.LocaleId;

            ClientIv = clientIv;
            ServerIv = serverIv;
        }
    }
}
