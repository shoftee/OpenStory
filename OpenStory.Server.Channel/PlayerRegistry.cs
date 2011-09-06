using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenStory.Server.Channel
{
    internal sealed class PlayerRegistry
    {
        // TODO: Get from DB later
        public const int UserLimit = 12;

        private readonly Dictionary<int, Player> players;
        private readonly Dictionary<string, int> nameLookup;

        public PlayerRegistry()
        {
            this.nameLookup = new Dictionary<string, int>(StringComparer.Ordinal);
            this.players = new Dictionary<int, Player>();
        }

        public int ClientCount
        {
            get { return this.players.Count; }
        }

        public void RegisterPlayer(Player player)
        {
            this.nameLookup.Add(player.CharacterName, player.CharacterId);
            this.players.Add(player.CharacterId, player);
        }

        public void UnregisterPlayer(Player player)
        {
            this.nameLookup.Remove(player.CharacterName);
            this.players.Remove(player.CharacterId);
        }

        public Player GetById(int characterId)
        {
            Player value;
            return this.players.TryGetValue(characterId, out value) ? value : null;
        }

        public Player GetByName(string name)
        {
            int id;
            return this.nameLookup.TryGetValue(name, out id) ? this.players[id] : null;
        }

        public IEnumerable<int> GetActive(IEnumerable<int> ids)
        {
            return ids.Intersect(this.players.Keys).ToList();
        }
    }
}