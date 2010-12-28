using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using OpenMaple.Constants;
using OpenMaple.Cryptography;
using OpenMaple.Data;
using OpenMaple.Networking;

namespace OpenMaple.Server.Login
{
    /// <summary>
    /// Represents a client for the Login Server.
    /// </summary>
    class LoginClient : AbstractClient
    {
        /// <summary>
        /// Denotes the maximum number of allowed failed login attempts before the client is disconnected.
        /// </summary>
        public const int MaxLoginAttempts = 3;

        /// <summary>
        /// Holds the number of failed login attempts for this client.
        /// </summary>
        public int LoginAttempts { get; private set; }

        /// <summary>
        /// Denotes whether the client has authenticated.
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Represents the point of the game connection process the client is currently in.
        /// </summary>
        public LoginClientState State { get; private set; }

        /// <summary>
        /// The key for this client, used to decrypt the password.
        /// </summary>
        public RSA RSAKey { get; private set; }

        private AccountSession accountSession;
        private ILoginServer loginServer;

        public override IAccount AccountInfo
        {
            get { return this.accountSession; }
        }

        /// <summary>
        /// Initializes a new instance of LoginClient and binds it with a network session.
        /// </summary>
        /// <param name="networkSession">The network session to bind the new LoginClient to.</param>
        public LoginClient(INetworkSession networkSession, ILoginServer loginServer)
            : base(networkSession)
        {
            this.LoginAttempts = 0;
            this.IsAuthenticated = false;
            this.State = LoginClientState.PreAuthentication;

            this.loginServer = loginServer;
        }

        /// <summary>
        /// Attempts to log into the system with a user name and password.
        /// </summary>
        /// <param name="userName">The user name of the account.</param>
        /// <param name="password">The password for the account.</param>
        /// <returns>true if login was successful; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">The exception is thrown if this method is called when <see cref="State" /> is not set to <see cref="LoginClientState.PreAuthentication"/>.</exception>
        /// <exception cref="ArgumentNullException">The exception is thrown if <paramref name="userName"/> or <paramref name="password"/> are null.</exception>
        public bool TryLogin(string userName, string password)
        {
            if (this.State != LoginClientState.PreAuthentication)
            {
                throw new InvalidOperationException("The client state should be 'PreAthentication' at login time.");
            }

            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            Account account = new Account(userName);
            if (account.AccountId != -1)
            {
                string hash = LoginCrypto.GetAuthenticationHash(userName, password);
                if (account.PasswordHash == hash)
                {
                    this.IsAuthenticated = true;
                }
            }

            bool success = this.IsAuthenticated;
            if (success)
            {
                this.accountSession = new AccountSession(account);

            }
            else
            {
                this.LoginAttempts++;
                if (this.LoginAttempts > MaxLoginAttempts)
                {
                    base.Disconnect();
                }
            }
            return success;
        }

        public IWorld SelectWorld(int worldId)
        {
            if (this.State != LoginClientState.WorldSelect && this.State != LoginClientState.ChannelSelect)
            {
                throw new InvalidOperationException("The client state should be 'WorldSelect' or 'ChannelSelect' at world selection time.");
            }
            return loginServer.GetWorldById(worldId);
        }

        public void SelectChannel(int worldId)
        {
            if (this.State != LoginClientState.ChannelSelect)
            {
                
            }
        }

        /// <summary>
        /// Checks if a character name is available for use.
        /// </summary>
        /// <param name="characterName">The name to check the availablitiy of.</param>
        /// <returns>true if the name is available for use. If the name is shorter than 4 or longer than 12 characters, or if it is already in use, false.</returns>
        /// <exception cref="ArgumentNullException">The exception is thrown if <paramref name="characterName"/> is null.</exception>
        public bool CheckName(string characterName)
        {
            if (this.State != LoginClientState.CharacterSelect)
            {
                throw new InvalidOperationException("The client state should be 'CharacterSelect' at name check time.");
            }

            if (characterName == null) throw new ArgumentNullException("characterName");
            if (characterName.Length < 4 || 12 < characterName.Length) return false;

            // TODO: Check for bad names.
            // Actually, the client checks for bad names, 
            // so if it got this far, go ahead and just ban.

            return CharacterEngine.IsNameAvailable(characterName);
        }
    }

    enum AuthenticationResult
    {
        Success = 0,
    }
}
