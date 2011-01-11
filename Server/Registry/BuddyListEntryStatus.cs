namespace OpenMaple.Server.Registry
{
    enum BuddyListEntryStatus
    {
        None = 0,
        /// <summary>
        /// This buddy list entry is still awaiting authorization by the target.
        /// </summary>
        Pending = 1,
        /// <summary>
        /// This buddy list entry is active, both characters are in each other's buddy lists.
        /// </summary>
        Active = 2,
        /// <summary>
        /// The remote character has deleted the entry from their buddy list, the buddy list entry with this status will never appear online.
        /// </summary>
        Inactive = 3
    }
}