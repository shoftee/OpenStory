namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Provides remote access to an <see cref="IAccountService"/>.
    /// </summary>
    public class AccountServiceClient : GameServiceClient<IAccountService>, IAccountService
    {
        /// <summary>
        /// Initializes a new AccountServiceClient.
        /// </summary>
        public AccountServiceClient()
            : base(ServerConstants.Uris.AccountService)
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
        /// Attempts to register a new session for the specified account.
        /// </summary>
        /// <param name="accountId">The account to register.</param>
        /// <param name="sessionId">A value-holder for the session identifier.</param>
        /// <returns><c>true</c> if the registration was successful; otherwise, <c>false</c>.</returns>
        public bool TryRegisterSession(int accountId, out int sessionId)
        {
            return base.Channel.TryRegisterSession(accountId, out sessionId);
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
        /// Attempts to remove the specified session from the list of active sessions.
        /// </summary>
        /// <param name="sessionId">The session to unregister.</param>
        /// <returns><c>true</c> if the session was removed successfully; otherwise, <c>false</c>.</returns>
        public bool TryUnregisterSession(int sessionId)
        {
            return base.Channel.TryUnregisterSession(sessionId);
        }

        #endregion
    }
}