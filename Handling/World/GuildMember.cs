using System;
using OpenMaple.Client;

namespace OpenMaple.Handling.World
{
    class GuildMember
    {
        public int GuildId { get; private set; }

        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public short Level { get; private set; }
        public int JobId { get; private set; }
        public GuildRank Rank { get; private set; }

        public bool IsOnline { get; private set; }
        public byte ChannelId { get; private set; }

        public GuildMember(Character character)
        {
            if (character == null) throw new ArgumentNullException("character");
            if (character.GuildId == -1)
            {
                throw new InvalidOperationException("This character doesn't belong to a guild.");
            }
            this.CharacterId = character.Id;
            this.Name = character.Name;
            this.Level = character.Level;
            this.JobId = character.JobId;
            this.Rank = character.GuildRank;
            this.ChannelId = character.Client.CurrentChannelId;
            this.GuildId = character.GuildId;
        }
    }
}