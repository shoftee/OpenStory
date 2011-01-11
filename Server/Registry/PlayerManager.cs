using System;
using System.Collections.Generic;
using System.Linq;
using OpenMaple.Game;
using OpenMaple.Networking;

namespace OpenMaple.Server.Registry
{
    sealed class PlayerManager
    {
        // TODO: Get from DB later
        public const int UserLimit = 12;

        private readonly Dictionary<string, IPlayer> playersByName;
        private readonly Dictionary<int, IPlayer> playersById;

        public int ClientCount { get { return this.playersById.Count; } }

        public PlayerManager()
        {
            // TODO: Pruning task?
            // TODO: Localization?
            this.playersByName = new Dictionary<string, IPlayer>(StringComparer.OrdinalIgnoreCase);
            this.playersById = new Dictionary<int, IPlayer>();
        }

        public void RegisterPlayer(IPlayer player)
        {
            this.playersByName.Add(player.CharacterName, player);
            this.playersById.Add(player.CharacterId, player);
        }

        public void UnregisterPlayer(IPlayer player)
        {
            this.playersByName.Remove(player.CharacterName);
            this.playersById.Remove(player.CharacterId);
        }

        public IPlayer GetById(int characterId)
        {
            IPlayer value;
            return this.playersById.TryGetValue(characterId, out value) ? value : null;
        }
    }
}
