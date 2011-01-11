using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenMaple.Constants;

namespace OpenMaple.Server.Registry
{
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
            var member = new GuildMember(player, this.Id);
            return members.Add(member);
        }
    }
}
