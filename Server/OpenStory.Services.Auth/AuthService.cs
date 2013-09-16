using System.ServiceModel;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Auth;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    sealed class AuthService : GameServer//, IAuthService
    {
        public AuthService(IServerProcess serverProcess, AuthOperator serverOperator)
            : base(serverProcess, serverOperator)
        {
        }
    }
}
