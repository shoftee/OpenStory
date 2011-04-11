using System;
using System.ServiceModel;
using OpenStory.Server.Common;

namespace OpenStory.AccountService
{
    /// <summary>
    /// Provides remote access to an <see cref="IAccountService"/>.
    /// </summary>
    public class AccountServiceClient : GameServiceClient<IAccountService>, IAccountService
    {
        /// <summary>
        /// Initializes a new AccountServiceClient.
        /// </summary>
        public AccountServiceClient() : base(ServerConstants.AccountServiceUri)
        {
        }

        #region IAccountService Members

        /// <summary>
        /// Checks whether there is an active session on the specified account.
        /// </summary>
        /// <param name="accountId">The account to check for.</param>
        /// <returns>true if the account is active; otherwise, false.</returns>
        public bool IsActive(int accountId)
        {
            return base.Channel.IsActive(accountId);
        }

        /// <summary>
        /// Registers a new session on the specified account and returns a session identifier.
        /// </summary>
        /// <param name="accountId">The account to register.</param>
        /// <returns>an identifier for the new session.</returns>
        public int RegisterSession(int accountId)
        {
            return base.Channel.RegisterSession(accountId);
        }

        /// <summary>
        /// Registers a character session on the specified account session.
        /// </summary>
        /// <param name="sessionId">The session on which to register a character.</param>
        /// <param name="characterId">The character to register.</param>
        public void RegisterCharacter(int sessionId, int characterId)
        {
            base.Channel.RegisterCharacter(sessionId, characterId);
        }

        /// <summary>
        /// Removes the specified session from the list of active sessions.
        /// </summary>
        /// <param name="sessionId">The session to unregister.</param>
        public void UnregisterSession(int sessionId)
        {
            base.Channel.UnregisterSession(sessionId);
        }

        #endregion
    }
}