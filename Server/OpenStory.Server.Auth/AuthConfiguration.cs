using System.Net;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents an AuthServer configuration.
    /// </summary>
    public sealed class AuthConfiguration : ServerConfiguration
    {
        /// <inheritdoc />
        public AuthConfiguration(IPEndPoint endpoint)
            : base(endpoint)
        {
        }
    }
}
