using System;

namespace OpenMaple.Server.Registry
{
    public class PartyMember : IEquatable<PartyMember>
    {
        public int CharacterId { get; private set; }
        public string CharacterName { get; private set; }

        public int Level { get; private set; }
        public int ChannelId { get; private set; }
        public int JobId { get; private set; }
        public int MapId { get; private set; }
        public bool IsOnline { get; private set; }

        public PartyMember(IPlayer player)
        {
            this.IsOnline = true;

            this.CharacterId = player.CharacterId;
            this.CharacterName = player.CharacterName;
            this.Level = player.Level;
            this.ChannelId = player.ChannelId;
            this.JobId = player.JobId;
            this.MapId = player.MapId;
        }

        public bool Equals(PartyMember other)
        {
            return this.CharacterId == other.CharacterId;
        }
    }
}