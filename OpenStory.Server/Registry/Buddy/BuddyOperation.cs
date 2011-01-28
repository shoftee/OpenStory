using System;

namespace OpenStory.Server.Registry.Buddy
{
    /// <summary>
    /// A buddy operation type.
    /// </summary>
    [Serializable]
    public enum BuddyOperation
    {
        /// <summary>
        /// Adding someone to a buddy list.
        /// </summary>
        AddBuddy = 1,
        /// <summary>
        /// Accepting someone else's AddBuddy request.
        /// </summary>
        AcceptRequest = 2,
        /// <summary>
        /// Removing someone from a buddy list.
        /// </summary>
        RemoveBuddy = 3,
    }
}