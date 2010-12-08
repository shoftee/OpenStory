using System;
using System.Collections.Concurrent;
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


    sealed class GuildManager
    {
        private static readonly GuildManager Instance = new GuildManager();

        private ConcurrentDictionary<int, Guild> guildCache;

        private GuildManager()
        {
            this.guildCache = new ConcurrentDictionary<int, Guild>();
        }

        private static Guild LoadGuild(int guildId)
        {
            // TODO: Finish this when I start doing the database crap.
            throw new NotImplementedException();
        }

        public static IGuild CreateGuild(int leaderId, string guildName)
        {
            // TODO: Finish this when I start doing the database crap.
            throw new NotImplementedException();
        }

        #region Guild nested class

        class Guild : IGuild
        {
            private static readonly Dictionary<GuildRank, string> DefaultRankTitles = new Dictionary<GuildRank, string>
            {
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
            public int Id { get; private set; }
            public int MasterCharacterId { get; private set; }

            public GuildEmblem Emblem { get; private set; }

            public string Notice { get; set; }
            public int GuildPoints { get; set; }
            public int Capacity { get; set; }

            private Guild()
            {
                rankTitles = new Dictionary<GuildRank, string>(5);
            }

            public Guild(int id)
                : this()
            {
                this.Id = id;
                // TODO: Load from database here?
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

            public void SetRankTitle(GuildRank rank, string newTitle)
            {
                if (!this.rankTitles.ContainsKey(rank))
                {
                    throw new ArgumentOutOfRangeException("rank");
                }
                this.rankTitles[rank] = newTitle;
            }
        }

        #endregion
    }

    internal interface IGuild
    {
        int Id { get; }
        string Name { get; }
        int MasterCharacterId { get; }
        GuildEmblem Emblem { get; }
        int GuildPoints { get; set; }
        int Capacity { get; set; }

        string GetRankTitle(GuildRank rank);
        void SetRankTitle(GuildRank rank, string newTitle);
    }
}
