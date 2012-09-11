using System.Net;
using System.ServiceModel;
using OpenStory.Server.Auth;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    /// <summary>
    /// Represents a WCF service that hosts the Authentication Server instance.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class AuthService : GameServiceBase, IAuthService
    {
        private readonly AuthServer server;

        public AuthService()
        {
            var config = new AuthConfiguration(IPAddress.Any, 8484);
            this.server = new AuthServer(config);
        }

        protected override void OnStarting()
        {
            this.server.Start();
        }

        protected override void OnStopping()
        {
            this.server.Stop();
        }
    }
}
