using System;
using System.Collections.Generic;

namespace OpenStory.Server.Registry
{
    internal sealed class PlayerRegistry
    {
        // TODO: Get from DB later
        public const int UserLimit = 12;

        private readonly Dictionary<int, IPlayer> playersById;
        private readonly Dictionary<string, IPlayer> playersByName;

        public PlayerRegistry()
        {
            // TODO: Pruning task?
            // TODO: Localization?
            this.playersByName = new Dictionary<string, IPlayer>(StringComparer.OrdinalIgnoreCase);
            this.playersById = new Dictionary<int, IPlayer>();
        }

        public int ClientCount
        {
            get { return this.playersById.Count; }
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

        public IPlayer GetById(string name)
        {
            IPlayer value;
            return this.playersByName.TryGetValue(name, out value) ? value : null;
        }
    }
}