using System;

namespace OpenStory.Server.AccountService
{
    /// <summary>
    /// Provides methods for manipulating account sessions.
    /// </summary>
    public interface IAccountSession : IDisposable
    {
        int AccountId { get; }

        string AccountName { get; }
    }

}
