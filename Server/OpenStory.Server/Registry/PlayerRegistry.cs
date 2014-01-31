using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenStory.Common;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Represents a registry for players.
    /// </summary>
    internal sealed class PlayerRegistry : IPlayerRegistry, IDisposable
    {
        private readonly Dictionary<int, CharacterKey> idLookup;
        private readonly Dictionary<string, CharacterKey> nameLookup;

        private readonly Dictionary<CharacterKey, IPlayer> players;

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

        public int Population
        {
            get { return this.l.ReadLock(() => this.players.Count); }
        }

        public PlayerRegistry()
        {
            this.idLookup = new Dictionary<int, CharacterKey>();
            this.nameLookup = new Dictionary<string, CharacterKey>();
            this.players = new Dictionary<CharacterKey, IPlayer>();

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
            return this.l.ReadLock(() => this.GetPlayerOrNull(name));
        }

        private IPlayer GetPlayerOrNull(int id)
        {
            CharacterKey key;
            if (!this.idLookup.TryGetValue(id, out key))
            {
                return default(IPlayer);
            }
            else
            {
                return this.GetPlayerOrNull(key);
            }
        }

        private IPlayer GetPlayerOrNull(string name)
        {
            CharacterKey key;
            if (!this.nameLookup.TryGetValue(name, out key))
            {
                return default(IPlayer);
            }
            else
            {
                return this.GetPlayerOrNull(key);
            }
        }

        private IPlayer GetPlayerOrNull(CharacterKey key)
        {
            IPlayer player;
            if (this.players.TryGetValue(key, out player))
            {
                return player;
            }
            else
            {
                return default(IPlayer);
            }
        }

        /// <inheritdoc />
        public IEnumerable<IPlayer> Scan(IEnumerable<CharacterKey> whitelist)
        {
            return this.l.ReadLock(() => this.GetByKeys(whitelist).ToList());
        }

        private IEnumerable<IPlayer> GetByKeys(IEnumerable<CharacterKey> ids)
        {
            return ids.Select(this.GetPlayerOrNull).Where(player => player != null);
        }

        private void AddPlayer(IPlayer player)
        {
            var key = player.Key;
            this.idLookup.Add(key.Id, key);
            this.nameLookup.Add(key.Name, key);
        }

        private void RemovePlayer(IPlayer player)
        {
            var key = player.Key;
            this.idLookup.Remove(key.Id);
            this.nameLookup.Remove(key.Name);
        }

        #region Implementation of IDisposable

        /// <inheritdoc />
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
        }

        #endregion
    }
}
