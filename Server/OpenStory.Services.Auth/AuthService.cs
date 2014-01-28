using System.ServiceModel;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Auth;
using OpenStory.Server.Processing;

namespace OpenStory.Services.Auth
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    sealed class AuthService : GameServer
    {
        private readonly NexusConnectionInfo nexusConnectionInfo;

        public AuthService(IServerProcess serverProcess, AuthOperator channelOperator, NexusConnectionInfo nexusConnectionInfo)
            : base(serverProcess, channelOperator)
        {
            this.nexusConnectionInfo = nexusConnectionInfo;
        }
    }
}
