using System;

namespace OpenStory.Server.Channel.Data
{
    /// <summary>
    /// Denotes the status of the friend relationship.
    /// </summary>
    [Serializable]
    public enum BuddyListEntryStatus
    {
        /// <summary>
        /// The buddy request has not been accepted by the other party.
        /// </summary>
        Pending,

        /// <summary>
        /// The entry is active and all is good.
        /// </summary>
        Active,

        /// <summary>
        /// The entry is inactive for whatever reason, e.g. the other party has removed their entry.
        /// </summary>
        Inactive,
    }
}