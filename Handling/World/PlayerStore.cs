using System;
using System.Collections.Generic;
using System.Linq;
using OpenMaple.Client;
using OpenMaple.Networking;

namespace OpenMaple.Handling.World
{
    sealed class PlayerStore
    {
        public const int MaxCharacters = 1200;

        private readonly Dictionary<string, Character> charactersByName;
        private readonly Dictionary<int, Character> charactersById;

        public int ClientCount { get { return charactersById.Count; } }

        public PlayerStore()
        {
            // TODO: Pruning task
            // TODO: Localization?
            charactersByName = new Dictionary<string, Character>(MaxCharacters, StringComparer.OrdinalIgnoreCase);
            charactersById = new Dictionary<int, Character>(MaxCharacters);
        }

        public void RegisterPlayer(Character character)
        {
            charactersByName.Add(character.Name.ToLowerInvariant(), character);
            charactersById.Add(character.Id, character);
        }

        public void UnregisterPlayer(Character character)
        {
            charactersByName.Remove(character.Name.ToLowerInvariant());
            charactersById.Remove(character.Id);
        }

        public Character GetById(int characterId)
        {
            Character value;
            if (!charactersById.TryGetValue(characterId, out value))
            {
                return null;
            }
            return value;
        }

        public void DisconnectAll()
        {
            foreach (var client in charactersById.Values.Where(c => !c.IsGameMaster).Select(c => c.Client))
            {
                client.Disconnect();
                client.Session.Close();
            }
        }

        public void Broadcast(IPacket packet)
        {
            foreach (var session in charactersById.Values.Select(c => c.Client.Session))
            {
                session.Write(packet);
            }
        }
    }
}
