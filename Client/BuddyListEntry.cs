using System;

namespace OpenMaple.Client
{
    class BuddyListEntry : IEquatable<BuddyListEntry>
    {
        private int channel;

        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public string Group { get; private set; }
        public bool IsOnline { get; set; }
        public bool IsVisible { get; set; }

        public int Channel
        {
            get { return IsOnline ? this.channel : -1; }
            private set { this.channel = value; }
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

        public BuddyListEntry(int characterId, string characterName, string buddyGroup, bool isOnline = false, int channel = -1, bool isVisible = false)
        {
            this.CharacterId = characterId;
            this.Name = characterName;
            this.Group = buddyGroup;
            this.IsOnline = isOnline;
            this.Channel = channel;
            this.IsVisible = isVisible;
        }
    }
}