using System;
using OpenStory.Cryptography;
using OpenStory.Server.Data;
using OpenStory.Server.Networking;

namespace OpenStory.Server.Login
{
    /// <summary>
    /// Represents a client for the Login Server.
    /// </summary>
    internal partial class LoginClient : AbstractClient
    {
        /// <summary>Denotes the maximum number of allowed failed login attempts before the client is disconnected.</summary>
        public const int MaxLoginAttempts = 3;

        private AccountSession accountSession;
        private ILoginServer loginServer;

        /// <summary>Initializes a new instance of LoginClient and binds it with a network session.</summary>
        /// <param name="networkSession">The network session to bind the new LoginClient to.</param>
        /// <param name="loginServer">The login server instance which is handling this client.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="loginServer"/> is <c>null</c>.</exception>
        public LoginClient(NetworkSession networkSession, ILoginServer loginServer)
            : base(networkSession)
        {
            if (loginServer == null) throw new ArgumentNullException("loginServer");
            this.LoginAttempts = 0;
            this.IsAuthenticated = false;
            this.State = LoginClientState.PreAuthentication;

            this.loginServer = loginServer;
        }

        /// <summary>Holds the number of failed login attempts for this client.</summary>
        public int LoginAttempts { get; private set; }

        /// <summary>Denotes whether the client has authenticated.</summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>Represents the point of the game connection process the client is currently in.</summary>
        public LoginClientState State { get; private set; }

        public override IAccount AccountInfo
        {
            get { return this.accountSession; }
        }

        /// <summary>Attempts to log into the system with a user name and password.</summary>
        /// <param name="userName">The user name of the account.</param>
        /// <param name="password">The password for the account.</param>
        /// <returns>true if login was successful; otherwise, false.</returns>
        /// <exception cref="InvalidOperationException">Thrown if this method is called when <see cref="State" /> is not set to <see cref="LoginClientState.PreAuthentication"/>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="userName"/> or <paramref name="password"/> are null.</exception>
        public bool TryLogin(string userName, string password)
        {
            if (this.State != LoginClientState.PreAuthentication)
            {
                throw new InvalidOperationException("The client state should be 'PreAthentication' at login time.");
            }

            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            AccountData accountData = AccountData.LoadByUserName(userName);
            if (accountData != null)
            {
                string hash = LoginCrypto.GetAuthenticationHash(userName, password);
                if (accountData.PasswordHash == hash)
                {
                    this.IsAuthenticated = true;
                }
            }

            bool success = this.IsAuthenticated;
            if (success)
            {
                this.accountSession = new AccountSession(accountData);
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
                throw new InvalidOperationException(
                    "The client state should be 'WorldSelect' or 'ChannelSelect' at world selection time.");
            }
            return this.loginServer.GetWorldById(worldId);
        }

        public void SelectChannel(int worldId)
        {
            if (this.State != LoginClientState.ChannelSelect) {}
        }

        /// <summary>Checks if a character name is available for use.</summary>
        /// <param name="characterName">The name to check the availablitiy of.</param>
        /// <returns>true if the name is available for use. If the name is shorter than 4 or longer than 12 characters, or if it is already in use, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="characterName"/> is <c>null</c>.</exception>
        public bool CheckName(string characterName)
        {
            // Now that I look at it, this method is pretty damn brutal, lol. Exceptions, exceptions, and then BAN. :D
            if (this.State != LoginClientState.CharacterSelect)
            {
                throw new InvalidOperationException("The client state should be 'CharacterSelect' at name check time.");
            }

            if (characterName == null) throw new ArgumentNullException("characterName");
            if (characterName.Length < 4 || 12 < characterName.Length)
            {
                throw new ArgumentException("The name to be checked must be between 4 and 12 characters in length.");
            }

            // TODO: Check for bad names.
            // The client already checks for bad names, 
            // so if it got this far, go ahead and just ban.

            return CharacterEngine.IsNameAvailable(characterName);
        }
    }
}