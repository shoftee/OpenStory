using System;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides information for an account session.
    /// </summary>
    public interface IAccountSession : IDisposable
    {
        /// <summary>
        /// Gets the ID for this session.
        /// </summary>
        int SessionId { get; }

        /// <summary>
        /// Gets the ID of the account.
        /// </summary>
        int AccountId { get; }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        string AccountName { get; }
    }
}