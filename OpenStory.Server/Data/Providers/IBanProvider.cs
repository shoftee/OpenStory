using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Data.Providers
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
        /// <param name="expiration">An optional expiration date and time. The default value is <c>null</c>.</param>
        /// <returns><c>true</c> if the ban was successful; otherwise, <c>false</c>.</returns>
        bool BanByAccountId(int accountId, string reason, DateTimeOffset? expiration = null);

        // TODO: more stuff to add here.
    }
}
