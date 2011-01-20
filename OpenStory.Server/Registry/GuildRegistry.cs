using System;
using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Synchronization;

namespace OpenStory.Server.Registry
{
    internal sealed class GuildRegistry : IGuildRegistry
    {
        private static readonly GuildRegistry Instance;
        private static readonly ISynchronized<GuildRegistry> SynchronizedInstance;

        private static readonly Dictionary<GuildRank, string> DefaultRankTitles;

        private Dictionary<int, Guild> guildCache;

        static GuildRegistry()
        {
            Instance = new GuildRegistry();
            SynchronizedInstance = Synchronizer.Synchronize(Instance);

            DefaultRankTitles = new Dictionary<GuildRank, string>
                                {
                                    { GuildRank.Master, "Master" },
                                    { GuildRank.JuniorMaster, "Jr.Master" },
                                    { GuildRank.HighMember, "Member" },
                                    { GuildRank.MediumMember, "Member" },
                                    { GuildRank.LowMember, "Member" }
                                };
        }

        private GuildRegistry()
        {
            this.guildCache = new Dictionary<int, Guild>();
        }

        public static ISynchronized<IGuildRegistry> Synchronized
        {
            get { return SynchronizedInstance; }
        }

        #region IGuildRegistry Members

        public IGuild CreateGuild(IPlayer master, string guildName)
        {
            // TODO: Finish this when I start doing the database crap.
            throw new NotImplementedException();
        }

        #endregion

        private Guild LoadGuild(int guildId)
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