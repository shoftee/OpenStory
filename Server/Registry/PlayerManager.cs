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

        private readonly Dictionary<string, Player> playersByName;
        private readonly Dictionary<int, Player> playersById;

        public int ClientCount { get { return this.playersById.Count; } }

        public PlayerManager()
        {
            // TODO: Pruning task?
            // TODO: Localization?
            this.playersByName = new Dictionary<string, Player>(UserLimit, StringComparer.OrdinalIgnoreCase);
            this.playersById = new Dictionary<int, Player>(UserLimit);
        }

        public void RegisterPlayer(Player player)
        {
            this.playersByName.Add(player.Character.Name.ToLowerInvariant(), player);
            this.playersById.Add(player.Character.Id, player);
        }

        public void UnregisterPlayer(Player character)
        {
            this.playersByName.Remove(character.Character.Name.ToLowerInvariant());
            this.playersById.Remove(character.Character.Id);
        }

        public Player GetById(int characterId)
        {
            Player value;
            if (!this.playersById.TryGetValue(characterId, out value))
            {
                return null;
            }
            return value;
        }

    }
}
