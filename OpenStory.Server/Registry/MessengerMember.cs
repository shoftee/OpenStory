using System;

namespace OpenStory.Server.Registry
{
    public class MessengerMember : IEquatable<MessengerMember>
    {
        public MessengerMember(IPlayer player, int position = 0)
        {
            this.Position = position;
            this.CharacterId = player.CharacterId;
            this.CharacterName = player.CharacterName;
            this.ChannelId = player.ChannelId;
        }

        public int CharacterId { get; private set; }
        public string CharacterName { get; private set; }

        public int Position { get; private set; }

        public int ChannelId { get; set; }

        #region IEquatable<MessengerMember> Members

        public bool Equals(MessengerMember other)
        {
            if (other == null) return false;
            return this.CharacterId == other.CharacterId;
        }

        #endregion
    }
}