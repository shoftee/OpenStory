using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

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
