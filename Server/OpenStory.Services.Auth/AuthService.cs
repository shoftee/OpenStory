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
    public sealed class AuthService : GameServiceBase, IAuthService
    {
        private AuthConfiguration serverConfiguration;

        private AuthServer server;

        /// <inheritdoc />
        protected override bool OnConfiguring(ServiceConfiguration configuration, out string error)
        {
            var endpoint = configuration.Get<IPEndPoint>("Endpoint");
            if (endpoint == null)
            {
                error = "Entry point address definition missing from configuration.";
                return false;
            }

            this.serverConfiguration = new AuthConfiguration(endpoint);

            error = null;
            return true;
        }

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            base.OnInitializing();

            this.server = new AuthServer(this.serverConfiguration);
        }

        /// <inheritdoc />
        protected override void OnStarting()
        {
            base.OnStarting();

            this.server.Start();
        }

        /// <inheritdoc />
        protected override void OnStopping()
        {
            this.server.Stop();

            base.OnStopping();
        }
    }
}
