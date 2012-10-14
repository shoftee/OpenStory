using System.Net;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents an AuthServer configuration.
    /// </summary>
    public sealed class AuthConfiguration : ServerConfiguration
    {
        /// <inheritdoc />
        public AuthConfiguration(IPAddress address, int port)
            : base(address, port)
        {
        }
    }
}
