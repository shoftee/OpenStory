using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Handling.World
{
    enum GuildRank
    {
        Master = 1,
        JrMaster = 2,
        HighMember = 3,
        MediumMember = 4,
        LowMember = 5
    }

    struct GuildEmblem
    {
        public int ForegroundId { get; set; }
        public byte ForegroundColor { get; set; }
        public int BackgroundId { get; set; }
        public byte BackgroundColor { get; set; }
    }

    class Guild
    {
        private static readonly Dictionary<GuildRank, string> DefaultRankTitles = new Dictionary<GuildRank, string> {
            {GuildRank.Master, "Master"},
            {GuildRank.JrMaster, "JrMaster"},
            {GuildRank.HighMember, "Member"},
            {GuildRank.MediumMember, "Member"},
            {GuildRank.LowMember, "Member"}
        };

        public static string GetDefaultRankTitle(GuildRank rank)
        {
            string value;
            if (!DefaultRankTitles.TryGetValue(rank, out value))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            return value;
        }

        private List<GuildMember> members;
        private readonly Dictionary<GuildRank, string> rankTitles;
        public string Name { get; private set; }
        public string Notice { get; private set; }

        public int GuildId { get; private set; }
        public int GuildPoints { get; private set; }

        public GuildEmblem Emblem { get; private set; }

        public int Signature { get; private set; }
        public int MasterCharacterId { get; private set; }
        public int Capacity { get; private set; }

        public Guild()
        {
            rankTitles = new Dictionary<GuildRank, string>(5);
        }

        public string GetRankTitle(GuildRank rank)
        {
            string value;
            if (!this.rankTitles.TryGetValue(rank, out value))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            return value;
        }
        public void SetRankTitle(GuildRank rank, string title)
        {
            if (!this.rankTitles.ContainsKey(rank))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            this.rankTitles[rank] = title;
        }
    }
}
