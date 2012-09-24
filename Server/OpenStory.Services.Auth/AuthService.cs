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
        private AuthServer server;

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            base.OnInitializing();

            var config = new AuthConfiguration(IPAddress.Any, 8484);
            this.server = new AuthServer(config);
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

        #region Overrides of GameServiceBase

        public override bool Configure(ServiceConfiguration configuration, out string error)
        {
            error = null;
            return true;
        }

        #endregion
    }
}
