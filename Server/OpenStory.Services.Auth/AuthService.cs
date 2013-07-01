using System.ServiceModel;
using OpenStory.Common.Tools;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Auth;
using OpenStory.Server.Fluent;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    /// <summary>
    /// Represents a WCF service that hosts the Authentication Server instance.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class AuthService : GameServiceBase, IAuthService
    {
        private AuthServer server;

        /// <inheritdoc />
        protected override bool OnConfiguring(ServiceConfiguration configuration, out string error)
        {
            var configurator = OS.Get<IServerConfigurator>();
            return configurator.CheckConfiguration(configuration, out error);
        }

        /// <inheritdoc />
        protected override void OnInitializing()
        {
            base.OnInitializing();

            this.server = OS.Get<AuthServer>();
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
