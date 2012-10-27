using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenStory.Common.Tools;

namespace OpenStory.Server.Registry
{
    internal sealed class PlayerRegistry : IPlayerRegistry, IDisposable
    {
        private readonly Dictionary<int, IPlayer> playerIdLookup;
        private readonly Dictionary<string, IPlayer> playerNameLookup;

        private bool isDisposed;
        private readonly ReaderWriterLockSlim @lock;

        public IPlayer this[int id]
        {
            get { return this.GetById(id); }
        }

        public IPlayer this[string name]
        {
            get { return this.GetByName(name); }
        }

        public PlayerRegistry()
        {
            this.playerIdLookup = new Dictionary<int, IPlayer>();
            this.playerNameLookup = new Dictionary<string, IPlayer>();

            this.@lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        /// <inheritdoc />
        public void RegisterPlayer(IPlayer player)
        {
            this.@lock.WriteLock(l => this.AddPlayer(player));
        }

        /// <inheritdoc />
        public void UnregisterPlayer(IPlayer player)
        {
            this.@lock.WriteLock(l => this.RemovePlayer(player));
        }

        /// <inheritdoc />
        public IPlayer GetById(int id)
        {
            var player = default(IPlayer);
            this.@lock.ReadLock(l => this.playerIdLookup.TryGetValue(id, out player));
            return player;
        }

        /// <inheritdoc />
        public IPlayer GetByName(string name)
        {
            var player = default(IPlayer);
            this.@lock.ReadLock(l => this.playerNameLookup.TryGetValue(name, out player));
            return player;
        }

        /// <inheritdoc />
        public void ScanLocked(Action<IEnumerable<IPlayer>> action)
        {
            this.@lock.ReadLock(l => action(this.playerIdLookup.Values));
        }

        /// <inheritdoc />
        public void ScanCopied(Action<IEnumerable<IPlayer>> action)
        {
            var players = new List<IPlayer>(0);
            this.@lock.ReadLock(l => players = this.playerIdLookup.Values.ToList());
            action(players);
        }

        private void AddPlayer(IPlayer player)
        {
            this.playerIdLookup.Add(player.CharacterId, player);
            this.playerNameLookup.Add(player.CharacterName, player);
        }

        private void RemovePlayer(IPlayer player)
        {
            this.playerIdLookup.Remove(player.CharacterId);
            this.playerNameLookup.Remove(player.CharacterName);
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                if (this.@lock != null)
                {
                    this.@lock.Dispose();
                }

                this.isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
