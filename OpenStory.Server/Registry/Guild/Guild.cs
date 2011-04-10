using System;
using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Common.Game;

namespace OpenStory.Server.Registry.Guild
{
    internal class Guild : IGuild
    {
        public const int DefaultCapacity = 10;

        private readonly Dictionary<GuildRank, string> rankTitles;
        private HashSet<GuildMember> members;

        public Guild()
        {
            this.rankTitles = new Dictionary<GuildRank, string>(5);
            this.members = new HashSet<GuildMember>();
        }

        #region IGuild Members

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int MasterCharacterId { get; private set; }

        public GuildEmblem Emblem { get; private set; }

        public bool IsFull
        {
            get { return this.members.Count == this.Capacity; }
        }

        public string Notice { get; set; }
        public int GuildPoints { get; set; }
        public int Capacity { get; set; }

        public string GetRankTitle(GuildRank rank)
        {
            if (!Enum.IsDefined(typeof (GuildRank), rank))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            return this.rankTitles[rank];
        }

        public void SetRankTitle(GuildRank rank, string newTitle)
        {
            if (!Enum.IsDefined(typeof (GuildRank), rank))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            this.rankTitles[rank] = newTitle;
        }

        public bool AddGuildMember(IPlayer player)
        {
            if (this.IsFull) return false;
            var member = new GuildMember(player, this.Id);
            return this.members.Add(member);
        }

        #endregion
    }
}