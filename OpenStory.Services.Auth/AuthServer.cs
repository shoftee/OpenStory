using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using OpenStory.Common.Authentication;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Server;
using OpenStory.Server.Data;
using OpenStory.Common.Data;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    /// <summary>
    /// Represents a server that handles the authentication process.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    sealed class AuthServer : AbstractServer, IAuthServer, IAuthService
    {
        private const string ServerName = "Auth";

        private static readonly AuthServerPackets PacketTableInternal = new AuthServerPackets();
        public static IOpCodeTable PacketTable { get { return PacketTableInternal; } }

        public override string Name { get { return ServerName; } }

        private readonly List<AuthClient> clients;
        private readonly List<IWorld> worlds;
        private readonly IAccountService accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthServer"/> class.
        /// </summary>
        public AuthServer()
            : base(IPAddress.Any, 8484)
        {
            this.worlds = new List<IWorld>();
            this.clients = new List<AuthClient>();
            this.accountService = new AccountServiceClient();
        }

        #region IAuthServer Members

        /// <inheritdoc />
        public IWorld GetWorldById(int worldId)
        {
            base.ThrowIfNotRunning();
            return this.worlds.First(w => w.Id == worldId);
        }

        /// <inheritdoc />
        public AuthenticationResult Authenticate(string accountName, string password, out IAccountSession accountSession)
        {
            base.ThrowIfNotRunning();
            Account account = Account.LoadByUserName(accountName);
            if (account == null)
            {
                return MiscTools.FailWithResult(out accountSession, AuthenticationResult.NotRegistered);
            }

            string hash = LoginCrypto.GetMd5HashString(password, true);
            if (!String.Equals(hash, account.PasswordHash, StringComparison.Ordinal))
            {
                return MiscTools.FailWithResult(out accountSession, AuthenticationResult.IncorrectPassword);
            }

            int sessionId;
            if (!this.accountService.TryRegisterSession(account.AccountId, out sessionId))
            {
                return MiscTools.FailWithResult(out accountSession, AuthenticationResult.AlreadyLoggedIn);
            }

            accountSession = base.GetSession(this.accountService, sessionId, account);
            return AuthenticationResult.Success;
        }

        #endregion

        protected override void OnConnectionOpen(ServerSession serverSession)
        {
            var newClient = new AuthClient(serverSession, this);
            this.clients.Add(newClient);
        }

        /// <inheritdoc />
        public bool Ping()
        {
            return this.IsRunning;
        }
    }
}
