using System;

namespace OpenStory.Server.Registry.Buddy
{
    /// <summary>
    /// The result of a buddy list operation.
    /// </summary>
    [Serializable]
    public enum BuddyOperationResult : byte
    {
        /// <summary>
        /// Default result.
        /// </summary>
        None = 0,
        /// <summary>
        /// The player's buddy list is full.
        /// </summary>
        BuddyListFull = 11,
        /// <summary>
        /// The target player's buddy list is full.
        /// </summary>
        TargetBuddyListFull = 12,
        /// <summary>
        /// The target is already in the player's buddy list.
        /// </summary>
        AlreadyOnList = 13,
        /// <summary>
        /// The target character was not found.
        /// </summary>
        CharacterNotFound = 15,
        /// <summary>
        /// The operation was successful.
        /// </summary>
        Success
    }
}