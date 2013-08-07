using OpenStory.Common.IO;
using OpenStory.Framework.Contracts;

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
            this.Header = configuration.Header;
            this.Version = configuration.Version;
            this.Subversion = configuration.Subversion;
            this.LocaleId = configuration.LocaleId;

            this.ClientIv = clientIv;
            this.ServerIv = serverIv;
        }
    }
}
