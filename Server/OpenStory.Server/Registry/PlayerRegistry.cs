using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Registry
{
    class PlayerRegistry
    {
        private readonly Dictionary<int, IPlayer> playerIdLookup;
        private readonly Dictionary<string, int> playerNameLookup;

        public PlayerRegistry()
        {
            this.playerIdLookup = new Dictionary<int, IPlayer>();
            this.playerNameLookup = new Dictionary<string, int>();
        }
    }
}
