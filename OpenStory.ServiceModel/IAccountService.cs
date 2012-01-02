using System;
using System.ServiceModel;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Provides methods for accessing an account service.
    /// </summary>
    [ServiceContract(Namespace = null)]
    public interface IAccountService : IGameService
    {
        /// <summary>
        /// Checks whether there is an active session on the specified account.
        /// </summary>
        /// <param name="accountId">The account to check for.</param>
        /// <returns><c>true</c> if the account is active; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool IsActive(int accountId);

        /// <summary>
        /// Attempts to register a new session for the specified account.
        /// </summary>
        /// <param name="accountId">The account to register.</param>
        /// <param name="sessionId">A value-holder for the session identifier.</param>
        /// <returns><c>true</c> if the registration was successful; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryRegisterSession(int accountId, out int sessionId);

        /// <summary>
        /// Registers a character session on the specified account.
        /// </summary>
        /// <param name="accountId">The account on which to register a character.</param>
        /// <param name="characterId">The character to register.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void RegisterCharacter(int accountId, int characterId);

        /// <summary>
        /// Attempts to remove the specified account from the list of active accounts.
        /// </summary>
        /// <param name="accountId">The account to unregister the session of.</param>
        /// <returns><c>true</c> if unregistration was successful; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryUnregisterSession(int accountId);
    }
}