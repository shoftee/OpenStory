using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using OpenStory.Common.Auth;
using OpenStory.Common.Data;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Represents a server that handles the authentication process.
    /// </summary>
    public sealed class AuthServer : ServerBase, IAuthServer
    {
        private const string ServerName = "Auth";

        private static readonly AuthServerPackets OpCodesInternal = new AuthServerPackets();

        /// <inheritdoc />
        public IOpCodeTable OpCodes
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

        private readonly IAccountService accountService;
        private readonly SimpleAuthPolicy authPolicy;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServer"/> class.
        /// </summary>
        public AuthServer()
            : base(IPAddress.Any, 8484)
        {
            this.worlds = new List<IWorld>();
            this.clients = new List<AuthClient>();
            this.accountService = new AccountServiceClient();
            this.authPolicy = new SimpleAuthPolicy(this.accountService);
        }

        #region IAuthServer Members

        /// <inheritdoc />
        public IWorld GetWorldById(int worldId)
        {
            base.ThrowIfNotRunning();
            return this.worlds.First(w => w.Id == worldId);
        }

        /// <inheritdoc />
        public IAuthPolicy<SimpleCredentials> GetAuthPolicy()
        {
            base.ThrowIfNotRunning();
            return this.authPolicy;
        }

        #endregion

        /// <inheritdoc />
        protected override void OnConnectionOpen(ServerSession serverSession)
        {
            var newClient = new AuthClient(serverSession, this);
            this.clients.Add(newClient);
        }
    }
}
