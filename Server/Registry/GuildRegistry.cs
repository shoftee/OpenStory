using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using OpenMaple.Constants;
using OpenMaple.Tools;

namespace OpenMaple.Server.Registry
{
    struct GuildEmblem
    {
        public int ForegroundId { get; set; }
        public byte ForegroundColor { get; set; }
        public int BackgroundId { get; set; }
        public byte BackgroundColor { get; set; }
    }

    sealed partial class GuildRegistry
    {
        private static readonly Dictionary<GuildRank, string> DefaultRankTitles = new Dictionary<GuildRank, string>
            {
                {GuildRank.Master, "Master"},
                {GuildRank.JrMaster, "Jr.Master"},
                {GuildRank.HighMember, "Member"},
                {GuildRank.MediumMember, "Member"},
                {GuildRank.LowMember, "Member"}
            };

        public static string GetDefaultRankTitle(GuildRank rank)
        {
            string value;
            if (!DefaultRankTitles.TryGetValue(rank, out value))
            {
                Debug.Fail("'rank' doesn't have a valid GuildRank enumeration value.");
            }
            return value;
        }

        private static readonly GuildRegistry Instance = new GuildRegistry();
        private GuildRegistry()
        {
            this.guildCache = new ConcurrentDictionary<int, Guild>();
        }

        private ConcurrentDictionary<int, Guild> guildCache;


        private static Guild LoadGuild(int guildId)
        {
            // TODO: Finish this when I start doing the database crap.

            throw new NotImplementedException();
        }

        public static IGuild CreateGuild(Player master, string guildName)
        {
            // TODO: Finish this when I start doing the database crap.
            const string InsertGuildQuery = "INSERT INTO Guild (WorldId, Name, MasterCharacterId, TimeSignature) " +
                                       "VALUES(@worldId, @name, @masterId, @timeSignature)\r\n" +
                                       "SELECT CAST(@@IDENTITY AS INT)";
            throw new NotImplementedException();
        }
    }

    internal interface IGuild
    {
        int Id { get; }
        string Name { get; }
        int MasterCharacterId { get; }
        GuildEmblem Emblem { get; }

        string Notice { get; set; }
        int GuildPoints { get; set; }
        int Capacity { get; set; }

        bool IsFull { get; }

        string GetRankTitle(GuildRank rank);
        void SetRankTitle(GuildRank rank, string newTitle);
        bool AddGuildMember(Player player);
    }
}
