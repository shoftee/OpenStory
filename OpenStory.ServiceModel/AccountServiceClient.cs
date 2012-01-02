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

        /// <inheritdoc />
        public bool IsActive(int accountId)
        {
            return base.Channel.IsActive(accountId);
        }

        /// <inheritdoc />
        public bool TryRegisterSession(int accountId, out int sessionId)
        {
            return base.Channel.TryRegisterSession(accountId, out sessionId);
        }

        /// <inheritdoc />
        public void RegisterCharacter(int accountId, int characterId)
        {
            base.Channel.RegisterCharacter(accountId, characterId);
        }

        /// <inheritdoc />
        public bool TryUnregisterSession(int accountId)
        {
            return base.Channel.TryUnregisterSession(accountId);
        }

        #endregion
    }
}