using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    class AuthServer : GameServer, IAuthService
    {
        public AuthServer(IServerProcess serverProcess, IServerOperator channelOperator)
            : base(serverProcess, channelOperator)
        {
        }
    }
}
