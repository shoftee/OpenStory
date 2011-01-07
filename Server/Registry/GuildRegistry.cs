using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using OpenMaple.Constants;
using OpenMaple.Threading;

namespace OpenMaple.Server.Registry
{
    sealed class GuildRegistry
    {
        const string InsertGuildQuery = "INSERT INTO Guild (WorldId, Name, MasterCharacterId, TimeSignature) " +
                                   "VALUES(@worldId, @name, @masterId, @timeSignature)\r\n" +
                                   "SELECT CAST(@@IDENTITY AS INT)";

        private static readonly GuildRegistry Instance = new GuildRegistry();
        private static readonly ISynchronized<GuildRegistry> SynchronizedInstance = Synchronizer.Synchronize(Instance);
        private static readonly Dictionary<GuildRank, string> DefaultRankTitles;
        static GuildRegistry()
        {
            Instance = new GuildRegistry();
            DefaultRankTitles = new Dictionary<GuildRank, string>
            {
                {GuildRank.Master, "Master"},
                {GuildRank.JrMaster, "Jr.Master"},
                {GuildRank.HighMember, "Member"},
                {GuildRank.MediumMember, "Member"},
                {GuildRank.LowMember, "Member"}
            };

        }

        private Dictionary<int, Guild> guildCache;
        private GuildRegistry()
        {
            this.guildCache = new Dictionary<int, Guild>();
        }

        private Guild LoadGuild(int guildId)
        {
            // TODO: Finish this when I start doing the database crap.
            throw new NotImplementedException();
        }

        public IGuild CreateGuild(Player master, string guildName)
        {
            // TODO: Finish this when I start doing the database crap.
            throw new NotImplementedException();
        }

        public static string GetDefaultRankTitle(GuildRank rank)
        {
            string value;
            if (!DefaultRankTitles.TryGetValue(rank, out value))
            {
                throw new ArgumentOutOfRangeException("rank");
            }
            return value;
        }
    }
}
