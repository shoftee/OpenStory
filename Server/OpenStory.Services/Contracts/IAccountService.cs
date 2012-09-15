using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for accessing an account service.
    /// </summary>
    [ServiceContract(Namespace = null, Name = null)]
    public interface IAccountService : IGameService
    {
        /// <summary>
        /// Attempts to register a new session for the specified account.
        /// </summary>
        /// <param name="accountId">The account to register.</param>
        /// <param name="sessionId">A variable to hold the session identifier.</param>
        /// <returns><c>true</c> if registration was successful; if the session is already active, <c>false</c>.</returns>
        [OperationContract]
        bool TryRegisterSession(int accountId, out int sessionId);

        /// <summary>
        /// Attempts to register a character session on the specified account.
        /// </summary>
        /// <param name="accountId">The account identifier onto which to register a character.</param>
        /// <param name="characterId">The character identifier to register.</param>
        /// <returns><c>true</c> if registration was successful; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryRegisterCharacter(int accountId, int characterId);

        /// <summary>
        /// Attempts to remove the specified account from the list of active accounts.
        /// </summary>
        /// <param name="accountId">The identifier of the account to unregister.</param>
        /// <returns><c>true</c> if unregistration was successful; otherwise, <c>false</c>.</returns>
        [OperationContract]
        bool TryUnregisterSession(int accountId);

        /// <summary>
        /// Attempts to keep the session of the specified account alive.
        /// </summary>
        /// <param name="accountId">The identifier of the account.</param>
        /// <param name="lag">A variable to hold the lag since the last keep alive attempt.</param>
        /// <returns>
        /// <c>true</c> if the signal was received successfully and the account was active at that time; 
        /// <c>false</c> if the connection was broken or the account was not active.
        /// </returns>
        [OperationContract]
        bool TryKeepAlive(int accountId, out TimeSpan lag);
    }
}
