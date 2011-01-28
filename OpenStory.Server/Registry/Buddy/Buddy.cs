using System;

namespace OpenStory.Server.Registry.Buddy
{
    internal class Buddy : IEquatable<Buddy>
    {
        public Buddy(int characterId, string characterName, BuddyStatus status, string groupName,
                              int channel = -1, bool isVisible = false)
        {
            this.CharacterId = characterId;
            this.CharacterName = characterName;
            this.GroupName = groupName;
            this.Status = status;
            this.IsOnline = (channel == -1);
            this.ChannelId = channel;
            this.IsVisible = isVisible;
        }

        public int CharacterId { get; private set; }
        public string CharacterName { get; private set; }
        public BuddyStatus Status { get; set; }
        public string GroupName { get; set; }
        public int ChannelId { get; set; }
        public bool IsOnline { get; set; }
        public bool IsVisible { get; set; }

        #region Implementation of IEquatable<Buddy>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Buddy other)
        {
            return this.CharacterId == other.CharacterId;
        }

        #endregion

    }
}