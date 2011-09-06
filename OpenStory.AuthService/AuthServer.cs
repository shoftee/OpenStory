using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using OpenStory.AccountService;
using OpenStory.Common.Authentication;
using OpenStory.Cryptography;
using OpenStory.Server;
using OpenStory.Server.Data;
using OpenStory.ServiceModel;
using OpenStory.Common.Data;

namespace OpenStory.AuthService
{
    /// <summary>
    /// Represents a server that handles the authentication process.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    sealed class AuthServer : AbstractServer, IAuthServer
    {
        private const string ServerName = "Auth";

        private static readonly AuthServerPackets PacketTableInternal = new AuthServerPackets();
        public static IOpCodeTable PacketTable { get { return PacketTableInternal; } }

        public override string Name { get { return ServerName; } }

        private readonly List<AuthClient> clients;
        private readonly List<IWorld> worlds;
        private readonly AccountServiceClient accountService;

        /// <summary>
        /// Initializes a new instance of the AuthServer class.
        /// </summary>
        public AuthServer()
            : base(8484)
        {
            this.worlds = new List<IWorld>();
            this.clients = new List<AuthClient>();
            this.accountService = new AccountServiceClient();
        }

        #region IAuthServer Members

        /// <summary>
        /// Gets a <see cref="OpenStory.Common.Authentication.IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>An <see cref="OpenStory.Common.Authentication.IWorld"/> object which represents the world with the given ID.</returns>
        public IWorld GetWorldById(int worldId)
        {
            base.ThrowIfNotRunning();
            return this.worlds.First(w => w.Id == worldId);
        }

        public AuthenticationResult Authenticate(string accountName, string password, out IAccountSession accountSession)
        {
            Account account = Account.LoadByUserName(accountName);
            AuthenticationResult result;
            if (account == null)
            {
                result = AuthenticationResult.NotRegistered;
                goto AuthenticationFailed;
            }

            string hash = LoginCrypto.GetMD5HashString(password, true);
            if (!String.Equals(hash, account.PasswordHash, StringComparison.Ordinal))
            {
                result = AuthenticationResult.IncorrectPassword;
                goto AuthenticationFailed;
            }

            if (this.accountService.IsActive(account.AccountId))
            {
                result = AuthenticationResult.AlreadyLoggedIn;
                goto AuthenticationFailed;
            }

            int sessionId = this.accountService.RegisterSession(account.AccountId);
            accountSession = base.GetSession(this.accountService, sessionId, account);
            return AuthenticationResult.Success;

        AuthenticationFailed:
            accountSession = null;
            return result;
        }

        #endregion

        protected override void HandleConnectionOpen(ServerSession serverSession)
        {
            AuthClient newClient = new AuthClient(serverSession, this);
            this.clients.Add(newClient);
        }

        public bool Ping()
        {
            return this.IsRunning;
        }
    }
}
