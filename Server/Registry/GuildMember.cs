using System;
using OpenMaple.Constants;

namespace OpenMaple.Server.Registry
{
    class GuildMember : IEquatable<GuildMember>
    {
        public int CharacterId { get; private set; }
        public int GuildId { get; private set; }

        public string CharacterName { get; private set; }
        public int Level { get; private set; }
        public int JobId { get; private set; }
        public GuildRank Rank { get; private set; }

        public bool IsOnline { get; private set; }
        public int ChannelId { get; private set; }

        public GuildMember(IPlayer player, int guildId, bool isOnline = false, GuildRank guildRank = GuildRank.LowMember)
        {
            this.CharacterId = player.CharacterId;
            this.CharacterName = player.CharacterName;
            this.Level = player.Level;
            this.JobId = player.JobId;
            this.ChannelId = player.ChannelId;

            this.IsOnline = isOnline;
            this.GuildId = guildId;
            this.Rank = guildRank;
        }

        public bool Equals(GuildMember other)
        {
            return this.CharacterId == other.CharacterId;
        }
    }
}