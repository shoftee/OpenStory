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
        /// <returns>true if the account is active; otherwise, false.</returns>
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
        /// Registers a character session on the specified account session.
        /// </summary>
        /// <param name="sessionId">The session on which to register a character.</param>
        /// <param name="characterId">The character to register.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void RegisterCharacter(int sessionId, int characterId);

        /// <summary>
        /// Attempts to remove the specified session from the list of active sessions.
        /// </summary>
        /// <param name="sessionId">The session to unregister.</param>
        /// <returns><c>true</c> if the session was removed successfully; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryUnregisterSession(int sessionId);
    }
}