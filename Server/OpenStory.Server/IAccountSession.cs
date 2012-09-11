using System;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides information for an account session.
    /// </summary>
    public interface IAccountSession : IDisposable
    {
        /// <summary>
        /// Gets the identifier for this session.
        /// </summary>
        int SessionId { get; }

        /// <summary>
        /// Gets the identifier of the account.
        /// </summary>
        int AccountId { get; }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        string AccountName { get; }

        /// <summary>
        /// Attempts to keep the connection alive.
        /// </summary>
        /// <param name="lag">A variable to hold the lag since the last keep alive attempt.</param>
        /// <returns>
        /// <c>true</c> if the signal was received successfully and the account was active at that time; 
        /// <c>false</c> if the connection was broken or the account was not active.
        /// </returns>
        bool TryKeepAlive(out TimeSpan lag);
    }
}
