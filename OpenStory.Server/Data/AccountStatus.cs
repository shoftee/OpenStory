using System;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// The status of an account.
    /// </summary>
    [Serializable]
    public enum AccountStatus
    {
        /// <summary>
        /// The account has never been accessed.
        /// </summary>
        FirstRun = 0,
        /// <summary>
        /// The account has been accessed at least once.
        /// </summary>
        Active = 1,
        /// <summary>
        /// The account is blocked.
        /// </summary>
        Blocked = 2,
        /// <summary>
        /// The account has been deleted.
        /// </summary>
        Deleted = 3
    }
}