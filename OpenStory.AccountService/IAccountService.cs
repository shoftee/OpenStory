using System;
using System.ServiceModel;
using OpenStory.Server.Common;

namespace OpenStory.AccountService
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
        /// Registers a new session on the specified account and returns a session identifier.
        /// </summary>
        /// <param name="accountId">The account to register.</param>
        /// <returns>an identifier for the new session.</returns>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        int RegisterSession(int accountId);

        /// <summary>
        /// Registers a character session on the specified account session.
        /// </summary>
        /// <param name="sessionId">The session on which to register a character.</param>
        /// <param name="characterId">The character to register.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void RegisterCharacter(int sessionId, int characterId);

        /// <summary>
        /// Removes the specified session from the list of active sessions.
        /// </summary>
        /// <param name="sessionId">The session to unregister.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        void UnregisterSession(int sessionId);
    }
}