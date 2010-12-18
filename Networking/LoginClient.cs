using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Cryptography;
using OpenMaple.Data;

namespace OpenMaple.Networking
{
    /// <summary>
    /// Represents a client for the Login Server.
    /// </summary>
    class LoginClient : ClientBase
    {
        /// <summary>
        /// Holds the number of failed login attempts for this client.
        /// </summary>
        public int LoginAttempts { get; private set; }

        /// <summary>
        /// Denotes whether the client has authenticated.
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Represents the point of the login process the client is currently in.
        /// </summary>
        public LoginClientState State { get; private set; }

        /// <summary>
        /// Initializes a new instance of LoginClient with the given session.
        /// </summary>
        /// <param name="session">The network session for this client.</param>
        public LoginClient(ISession session)
            : base(session)
        {
            this.LoginAttempts = 0;
            this.IsAuthenticated = false;
            this.State = LoginClientState.PreAuthentication;
        }

        public bool TryLogin(string accountName, string password)
        {
            if (accountName == null) throw new ArgumentNullException("accountName");
            if (password == null) throw new ArgumentNullException("password");
            if (this.State != LoginClientState.PreAuthentication)
            {
                throw new InvalidOperationException("The client state should be at 'PreAthentication' at login time.");
            }
            bool success = false;
            Account account = new Account(accountName);
            if (account.AccountId != -1)
            {
                string hash =
                    LoginCrypto.GetAuthenticationHash(accountName, password);
                if (account.PasswordHash == hash)
                {
                    this.IsAuthenticated = true;
                    return true;
                }
            }
            return false;
        }
    }

    enum AuthenticationResult
    {
        None = 0
    }

    enum LoginClientState
    {
        None = 0,
        PreAuthentication,
        PostAuthentication,
        WorldSelect,
        ChannelSelect,
        CharacterSelect,
        CharacterCreate,
        PostCharacterSelect
    }
}
