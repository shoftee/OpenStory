using System.Collections.Generic;
using System.Linq;
using OpenStory.Common;
using OpenStory.Common.Game;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Auth.Policy;
using OpenStory.Server.Fluent;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents a server that handles the authentication process.
    /// </summary>
    public sealed class AuthServer : ServerBase, IAuthServer
    {
        private const string ServerName = @"Auth";

        private static readonly AuthServerPackets OpCodesInternal = new AuthServerPackets();

        /// <inheritdoc />
        protected override IPacketCodeTable OpCodes
        {
            get { return OpCodesInternal; }
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return ServerName; }
        }

        private readonly List<AuthClient> clients;
        private readonly List<IWorld> worlds;

        private readonly AuthConfiguration authConfiguration;
        private readonly IAuthPolicy<SimpleCredentials> authPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServer"/> class.
        /// </summary>
        public AuthServer(AuthConfiguration authConfiguration, IAuthPolicy<SimpleCredentials> authPolicy)
            : base(authConfiguration)
        {
            this.worlds = new List<IWorld>();
            this.clients = new List<AuthClient>();

            this.authConfiguration = authConfiguration;
            this.authPolicy = authPolicy;
        }

        #region IAuthServer Members

        /// <inheritdoc />
        public IWorld GetWorldById(int worldId)
        {
            this.ThrowIfNotRunning();
            return this.worlds.First(w => w.Id == worldId);
        }

        /// <inheritdoc />
        public IAuthPolicy<SimpleCredentials> GetAuthPolicy()
        {
            this.ThrowIfNotRunning();
            return this.authPolicy;
        }

        #endregion

        /// <inheritdoc />
        protected override void OnConnectionOpen(IServerSession serverSession)
        {
            var newClient = new AuthClient(this, serverSession);
            this.clients.Add(newClient); // NOTE: Happens both in Auth and Channel servers, pull up?
        }
    }
}
