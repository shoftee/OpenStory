using System;
using OpenMaple.Constants;

namespace OpenMaple.Server.Registry
{
    class GuildMember : IPlayerExtension
    {
        public int PlayerId { get; private set; }
        public int GuildId { get; private set; }

        public string Name { get; private set; }
        public int Level { get; private set; }
        public int JobId { get; private set; }
        public GuildRank Rank { get; private set; }

        public bool IsOnline { get { return this.ChannelId != -1; } }
        public int ChannelId { get; private set; }

        public GuildMember(IPlayer player, int guildId, int channelId = -1, GuildRank guildRank = GuildRank.LowMember)
        {
            this.PlayerId = player.CharacterId;
            this.GuildId = guildId;

            this.Name = player.CharacterName;
            this.Level = player.Level;
            this.JobId = player.JobId;
            this.Rank = guildRank;
            this.ChannelId = channelId;
        }

        public void Update(IPlayer player)
        {
            throw new NotImplementedException();
        }

        public void Release()
        {
            throw new NotImplementedException();
        }
    }
}