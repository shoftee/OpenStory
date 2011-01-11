using System;
using System.Collections.Generic;
using OpenMaple.Constants;
using OpenMaple.Synchronization;

namespace OpenMaple.Server.Registry
{
    sealed class GuildRegistry : IGuildRegistry
    {
        private static readonly GuildRegistry Instance;
        private static readonly ISynchronized<GuildRegistry> SynchronizedInstance;
        public static ISynchronized<IGuildRegistry> Synchronized
        {
            get { return SynchronizedInstance; }
        }

        private static readonly Dictionary<GuildRank, string> DefaultRankTitles;
        static GuildRegistry()
        {
            Instance = new GuildRegistry();
            SynchronizedInstance = Synchronizer.Synchronize(Instance);

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

        public IGuild CreateGuild(IPlayer master, string guildName)
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
