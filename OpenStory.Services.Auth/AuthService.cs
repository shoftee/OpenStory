using System.ServiceModel;
using OpenStory.Server.Auth;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    /// <summary>
    /// Represents a WCF service that hosts the Authentication Server instance.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal sealed class AuthService : IAuthService
    {
        private readonly AuthServer server;

        public AuthService()
        {
            this.server = new AuthServer();
        }

        #region IAuthService Members

        /// <inheritdoc/>
        public void Start()
        {
            this.server.Start();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            this.server.Stop();
        }

        /// <inheritdoc/>
        public bool Ping()
        {
            return true;
        }

        #endregion
    }
}
