using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenMaple.Constants;

namespace OpenMaple.Server.Registry
{
    class GuildEmblem
    {
        public int ForegroundId { get; set; }
        public byte ForegroundColor { get; set; }
        public int BackgroundId { get; set; }
        public byte BackgroundColor { get; set; }

        public GuildEmblem(int foregroundId, byte foregroundColor, int backgroundId, byte backgroundColor)
        {
            this.ForegroundId = foregroundId;
            this.ForegroundColor = foregroundColor;
            this.BackgroundId = backgroundId;
            this.BackgroundColor = backgroundColor;
        }
    }

    class Guild : IGuild
    {
        public const int DefaultCapacity = 10;

        private HashSet<GuildMember> members;
        private readonly Dictionary<GuildRank, string> rankTitles;
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int MasterCharacterId { get; private set; }

        public GuildEmblem Emblem { get; private set; }

        public bool IsFull { get { return this.members.Count == this.Capacity; } }

        public string Notice { get; set; }
        public int GuildPoints { get; set; }
        public int Capacity { get; set; }

        public Guild()
        {
            rankTitles = new Dictionary<GuildRank, string>(5);
            members = new HashSet<GuildMember>();
        }

        public string GetRankTitle(GuildRank rank)
        {
            if (!Enum.IsDefined(typeof(GuildRank), rank))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            return this.rankTitles[rank];
        }

        public void SetRankTitle(GuildRank rank, string newTitle)
        {
            if (!Enum.IsDefined(typeof(GuildRank), rank))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            this.rankTitles[rank] = newTitle;
        }

        public bool AddGuildMember(IPlayer player)
        {
            if (this.IsFull) return false;
            GuildMember member = new GuildMember(player, this.Id, player.ChannelId);
            return members.Add(member);
        }
    }

    interface IGuild
    {
        int Id { get; }
        string Name { get; }
        int MasterCharacterId { get; }
        GuildEmblem Emblem { get; }
        bool IsFull { get; }

        string Notice { get; set; }
        int GuildPoints { get; set; }
        int Capacity { get; set; }

        string GetRankTitle(GuildRank rank);
        void SetRankTitle(GuildRank rank, string newTitle);

        bool AddGuildMember(IPlayer player);
    }
}
