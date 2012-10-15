using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenStory.Common.Tools;

namespace OpenStory.Server.Registry
{
    internal sealed class PlayerRegistry
    {
        private readonly Dictionary<int, IPlayer> playerIdLookup;
        private readonly Dictionary<string, IPlayer> playerNameLookup;

        private readonly ReaderWriterLockSlim @lock;

        public IPlayer this[int id]
        {
            get
            {
                var player = default(IPlayer);
                this.@lock.ReadLock(l => this.playerIdLookup.TryGetValue(id, out player));
                return player;
            }
        }

        public IPlayer this[string name]
        {
            get
            {
                var player = default(IPlayer);
                this.@lock.ReadLock(l => this.playerNameLookup.TryGetValue(name, out player));
                return player;
            }
        }

        public PlayerRegistry()
        {
            this.playerIdLookup = new Dictionary<int, IPlayer>();
            this.playerNameLookup = new Dictionary<string, IPlayer>();

            this.@lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        public void RegisterPlayer(IPlayer player)
        {
            this.@lock.WriteLock(l => this.AddPlayer(player));
        }

        private void AddPlayer(IPlayer player)
        {
            this.playerIdLookup.Add(player.CharacterId, player);
            this.playerNameLookup.Add(player.CharacterName, player);
        }

        public void UnregisterPlayer(IPlayer player)
        {
            this.@lock.WriteLock(l => this.RemovePlayer(player));
        }

        private void RemovePlayer(IPlayer player)
        {
            this.playerIdLookup.Remove(player.CharacterId);
            this.playerNameLookup.Remove(player.CharacterName);
        }

        /// <summary>
        /// Runs a scan on the player instances, while locking the registry.
        /// </summary>
        /// <remarks>
        /// <para>The objects in the instance list will not become unregistered during the scan.</para>
        /// <para>The scan will however block write operations on the registry.</para>
        /// </remarks>
        /// <param name="action">The action to execute during the scan.</param>
        public void ScanLocked(Action<IEnumerable<IPlayer>> action)
        {
            this.@lock.ReadLock(l => action(this.playerIdLookup.Values));
        }

        /// <summary>
        /// Runs a scan on a copy of the player instance list.
        /// </summary>
        /// <remarks>
        /// <para>The objects in the instance list may become unregistered during the scan.</para>
        /// <para>The scan will not block write operations to the registry.</para>
        /// </remarks>
        /// <param name="action">The action to execute during the scan.</param>
        public void ScanCopied(Action<IEnumerable<IPlayer>> action)
        {
            List<IPlayer> players = new List<IPlayer>(0);
            this.@lock.ReadLock(l => players = this.playerIdLookup.Values.ToList());
            action(players);
        }
    }
}
