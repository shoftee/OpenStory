using System.ServiceModel;
using OpenStory.Server.Auth;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Auth
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    internal sealed class AuthService : GameServer
    {
        private readonly NexusConnectionInfo nexusConnectionInfo;

        public AuthService(IServerProcess serverProcess, AuthOperator authOperator, NexusConnectionInfo nexusConnectionInfo)
            : base(serverProcess, authOperator)
        {
            this.nexusConnectionInfo = nexusConnectionInfo;
        }
    }
}
