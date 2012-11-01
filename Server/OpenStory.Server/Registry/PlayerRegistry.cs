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
        private readonly ReaderWriterLockSlim l;

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

            this.l = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        /// <inheritdoc />
        public void RegisterPlayer(IPlayer player)
        {
            this.l.WriteLock(() => this.AddPlayer(player));
        }

        /// <inheritdoc />
        public void UnregisterPlayer(IPlayer player)
        {
            this.l.WriteLock(() => this.RemovePlayer(player));
        }

        /// <inheritdoc />
        public IPlayer GetById(int id)
        {
            return this.l.ReadLock(() => this.GetPlayerOrNull(id));
        }

        /// <inheritdoc />
        public IPlayer GetByName(string name)
        {
            return this.l.ReadLock(() => GetPlayerOrNull(name));
        }

        private IPlayer GetPlayerOrNull(int id)
        {
            var player = default(IPlayer);
            this.playerIdLookup.TryGetValue(id, out player);
            return player;
        }

        private IPlayer GetPlayerOrNull(string name)
        {
            var player = default(IPlayer);
            this.playerNameLookup.TryGetValue(name, out player);
            return player;
        }

        public IEnumerable<IPlayer> Scan(IEnumerable<int> whitelist)
        {
            return this.l.ReadLock(() => this.GetByIds(whitelist).ToList());
        }

        private IEnumerable<IPlayer> GetByIds(IEnumerable<int> ids)
        {
            return ids.Select(this.GetPlayerOrNull).Where(player => player != null);
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
                if (this.l != null)
                {
                    this.l.Dispose();
                }

                this.isDisposed = true;
            }

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
