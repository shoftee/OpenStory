using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenMaple.Constants;

namespace OpenMaple.Server.Registry
{
    partial class GuildRegistry
    {
        class Guild : IGuild
        {
            private HashSet<GuildMember> members;
            private readonly Dictionary<GuildRank, string> rankTitles;
            public int Id { get; private set; }
            public string Name { get; private set; }
            public int MasterCharacterId { get; private set; }

            public GuildEmblem Emblem { get; private set; }

            public string Notice { get; set; }
            public int GuildPoints { get; set; }
            public int Capacity { get; set; }

            public bool IsFull { get { return this.members.Count == this.Capacity; } }
            
            public Guild()
            {
                rankTitles = new Dictionary<GuildRank, string>(5);
                members = new HashSet<GuildMember>();
            }

            public string GetRankTitle(GuildRank rank)
            {

                string value;
                if (!this.rankTitles.TryGetValue(rank, out value))
                {
                    Debug.Fail("'rank' doesn't have a valid GuildRank enumeration value.");
                }
                return value;
            }

            public void SetRankTitle(GuildRank rank, string newTitle)
            {
                if (!this.rankTitles.ContainsKey(rank))
                {
                    Debug.Fail("'rank' doesn't have a valid GuildRank enumeration value.");
                }
                this.rankTitles[rank] = newTitle;
            }

            public bool AddGuildMember(Player player)
            {
                if (this.IsFull) return false;
                GuildMember member = new GuildMember(player, this.Id, player.ChannelId);
                return members.Add(member);
            }
        }
    }
}
