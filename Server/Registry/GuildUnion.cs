using System;
using System.Collections.Generic;
using OpenMaple.Constants;

namespace OpenMaple.Server.Registry
{
    class GuildUnion
    {
        private List<IGuild> guilds;
        private readonly Dictionary<GuildRank, string> rankTitles;

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Notice { get; set; }

        public static GuildUnion GetById(int allianceId)
        {
            // TODO: Database crap.
            throw new NotImplementedException();
        }
    }
}
