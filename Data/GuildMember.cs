using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Constants;
using OpenMaple.Server.Registry;

namespace OpenMaple.Data
{
    class GuildMember
    {
        private bool hasChanged;

        public int CharacterId { get; private set; }
        public int GuildId { get; private set; }
        public GuildRank Rank { get; private set; }
    }
}
