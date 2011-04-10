using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Authentication;
using OpenStory.Cryptography;
using OpenStory.Server.AccountService;
using OpenStory.Server.Data;

namespace OpenStory.Server.Emulation.Authentication
{
    /// <summary>
    /// Represents a server that handles the authentication process.
    /// </summary>
    sealed class AuthServer : AbstractServer, IAuthServer
    {
        /// <summary>
        /// Initial maximum number of characters on a single account.
        /// </summary>
        public const int MaxCharacters = 3;

        public override string Name { get { return "Auth"; } }

        private readonly List<AuthClient> clients;
        private readonly List<IWorld> worlds;

        private readonly HashSet<int> activeAccounts;

        /// <summary>
        /// Initializes a new instance of the AuthServer class.
        /// <param name="port">The port on which to listen for incoming connections. Default value is 8484.</param>
        /// </summary>
        public AuthServer(int port = 8484)
            : base(port)
        {
            this.worlds = new List<IWorld>();
            this.clients = new List<AuthClient>();
            this.activeAccounts = new HashSet<int>();
        }

        // TODO: FINISH THIS F5

        #region IAuthServer Members

        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>An <see cref="IWorld"/> object which represents the world with the given ID.</returns>
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

            if (Universe.Accounts.IsActive(account.AccountId))
            {
                result = AuthenticationResult.AlreadyLoggedIn;
                goto AuthenticationFailed;
            }

            accountSession = Universe.Accounts.RegisterSession(account);
            return AuthenticationResult.Success;

        AuthenticationFailed:
            accountSession = null;
            return result;
        }

        #endregion

        protected override void HandleSession(ServerSession serverSession)
        {
            AuthClient newClient = new AuthClient(serverSession, this);
            this.clients.Add(newClient);
        }

    }
}