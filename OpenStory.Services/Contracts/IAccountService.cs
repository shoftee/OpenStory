using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for accessing an account service.
    /// </summary>
    [ServiceContract(Namespace = null)]
    public interface IAccountService : IGameService
    {
        /// <summary>
        /// Attempts to register a new session for the specified account.
        /// </summary>
        /// <param name="accountId">The account to register.</param>
        /// <param name="sessionId">A value-holder for the session identifier.</param>
        /// <returns><c>true</c> if registration was successful; if the session is already active, <c>false</c>.</returns>
        [OperationContract]
        bool TryRegisterSession(int accountId, out int sessionId);

        /// <summary>
        /// Attempts to register a character session on the specified account.
        /// </summary>
        /// <param name="accountId">The account on which to register a character.</param>
        /// <param name="characterId">The character to register.</param>
        /// <returns><c>true</c> if registration was successful; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryRegisterCharacter(int accountId, int characterId);

        /// <summary>
        /// Attempts to remove the specified account from the list of active accounts.
        /// </summary>
        /// <param name="accountId">The account to unregister the session of.</param>
        /// <returns><c>true</c> if unregistration was successful; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryUnregisterSession(int accountId);
    }
}
