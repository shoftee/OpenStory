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
        /// <returns><c>true</c> if the account is active; otherwise, <c>false</c>.</returns>
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
        /// Registers a character session on the specified account.
        /// </summary>
        /// <param name="accountId">The account on which to register a character.</param>
        /// <param name="characterId">The character to register.</param>
        public void RegisterCharacter(int accountId, int characterId)
        {
            base.Channel.RegisterCharacter(accountId, characterId);
        }

        /// <summary>
        /// Attempts to remove the specified account from the list of active accounts.
        /// </summary>
        /// <param name="accountId">The account to unregister the session of.</param>
        /// <returns><c>true</c> if unregistration was successful; otherwise, <c>false</c>.</returns>
        public bool TryUnregisterSession(int accountId)
        {
            return base.Channel.TryUnregisterSession(accountId);
        }

        #endregion
    }
}