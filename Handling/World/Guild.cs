using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Client;

namespace OpenMaple.Handling.World
{
    struct GuildEmblem
    {
        public int ForegroundId { get; set; }
        public byte ForegroundColor { get; set; }
        public int BackgroundId { get; set; }
        public byte BackgroundColor { get; set; }
    }

    enum GuildRank
    {
        Unknown = 0,
        Master = 1,
        JrMaster = 2,
        HighMember = 3,
        MediumMember = 4,
        LowMember = 5
    }

    class Guild
    {
        private List<GuildCharacter> members;
        private readonly string[] RankTitles = new string[5];
        public string Name { get; private set; }
        public string Notice { get; private set; }

        public int GuildId { get; private set; }
        public int GuildPoints { get; private set; }

        public GuildEmblem Emblem { get; private set; }

        public int Signature { get; private set; }
        public int MasterCharacterId { get; private set; }
        public int Capacity { get; private set; }
    }

    class GuildCharacter
    {
        public int GuildId { get; private set; }

        public int CharacterId { get; private set; }
        public string Name { get; private set; }
        public short Level { get; private set; }
        public int JobId { get; private set; }
        public GuildRank Rank { get; private set; }

        public bool IsOnline { get; private set; }
        public byte ChannelId { get; private set; }
        
        public GuildCharacter(Character character)
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
