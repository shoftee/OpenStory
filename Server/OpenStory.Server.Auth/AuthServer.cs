using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents an auth server.
    /// </summary>
    public class AuthServer : NetworkServer<AuthOperator>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServer"/> class.
        /// </summary>
        public AuthServer(IServerProcess process, AuthOperator @operator)
            : base(process, @operator)
        {
        }
    }
}
