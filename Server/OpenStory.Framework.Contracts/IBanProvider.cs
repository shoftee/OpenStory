using System;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for operating with bans.
    /// </summary>
    public interface IBanProvider
    {
        /// <summary>
        /// Bans an account by its identifier.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="reason">A reason for the ban.</param>
        /// <param name="expiration">An optional expiration date and time. The default value is <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the ban was successful; otherwise, <see langword="false"/>.</returns>
        bool BanByAccountId(int accountId, string reason, DateTimeOffset? expiration = null);

        // TODO: more stuff to add here.
    }
}
