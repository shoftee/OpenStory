using System;

namespace OpenMaple.Game
{
    class BuddyListEntry : IEquatable<BuddyListEntry>
    {
        private int channel;

        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public BuddyListEntryStatus Status { get; set; }
        public string GroupName { get; set; }
        public int Channel { get; set; }
        public bool IsOnline { get; set; }
        public bool IsVisible { get; set; }

        public BuddyListEntry(int characterId, string characterName, BuddyListEntryStatus status, string groupName, int channel = -1, bool isVisible = false)
        {
            this.CharacterId = characterId;
            this.Name = characterName;
            this.GroupName = groupName;
            this.Status = status;
            this.IsOnline = (channel == -1);
            this.Channel = channel;
            this.IsVisible = isVisible;
        }

        #region Implementation of IEquatable<BuddyListEntry>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(BuddyListEntry other)
        {
            return this.CharacterId == other.CharacterId;
        }

        #endregion
    }

    enum BuddyListEntryStatus
    {
        None = 0,
        /// <summary>
        /// This buddy list entry is still awaiting authorization by the target.
        /// </summary>
        PendingRequest = 1,
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