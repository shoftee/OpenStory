namespace OpenStory.Server.Registry.Buddy
{
    internal enum BuddyStatus
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,
        /// <summary>
        /// This entry is still awaiting authorization by the target.
        /// </summary>
        Pending = 1,
        /// <summary>
        /// This entry is active, both characters are in each other's buddy lists.
        /// </summary>
        Active = 2,
        /// <summary>
        /// The remote character has deleted the entry from their buddy list, the buddy entry with this status will never appear online.
        /// </summary>
        Inactive = 3
    }
}