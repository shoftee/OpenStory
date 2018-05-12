using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenStory.Common;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a registry for players.
    /// </summary>
    internal sealed class PlayerRegistry : IPlayerRegistry, IDisposable
    {
        private readonly Dictionary<int, CharacterKey> _idLookup;
        private readonly Dictionary<string, CharacterKey> _nameLookup;

        private readonly Dictionary<CharacterKey, IPlayer> _players;

        private bool _isDisposed;
        private ReaderWriterLockSlim _l;

        public IPlayer this[int id] => GetById(id);

        public IPlayer this[string name] => GetByName(name);

        public int Population
        {
            get { return _l.ReadLock(() => _players.Count); }
        }

        public PlayerRegistry()
        {
            _idLookup = new Dictionary<int, CharacterKey>();
            _nameLookup = new Dictionary<string, CharacterKey>();
            _players = new Dictionary<CharacterKey, IPlayer>();

            _l = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        /// <inheritdoc />
        public void RegisterPlayer(IPlayer player)
        {
            _l.WriteLock(() => AddPlayer(player));
        }

        /// <inheritdoc />
        public void UnregisterPlayer(IPlayer player)
        {
            _l.WriteLock(() => RemovePlayer(player));
        }

        /// <inheritdoc />
        public IPlayer GetById(int id)
        {
            return _l.ReadLock(() => GetPlayerOrNull(id));
        }

        /// <inheritdoc />
        public IPlayer GetByName(string name)
        {
            return _l.ReadLock(() => GetPlayerOrNull(name));
        }

        private IPlayer GetPlayerOrNull(int id)
        {
            CharacterKey key;
            if (!_idLookup.TryGetValue(id, out key))
            {
                return default(IPlayer);
            }
            else
            {
                return GetPlayerOrNull(key);
            }
        }

        private IPlayer GetPlayerOrNull(string name)
        {
            CharacterKey key;
            if (!_nameLookup.TryGetValue(name, out key))
            {
                return default(IPlayer);
            }
            else
            {
                return GetPlayerOrNull(key);
            }
        }

        private IPlayer GetPlayerOrNull(CharacterKey key)
        {
            IPlayer player;
            if (_players.TryGetValue(key, out player))
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
            return _l.ReadLock(() => GetByKeys(whitelist).ToList());
        }

        private IEnumerable<IPlayer> GetByKeys(IEnumerable<CharacterKey> ids)
        {
            return ids.Select(GetPlayerOrNull).Where(player => player != null);
        }

        private void AddPlayer(IPlayer player)
        {
            var key = player.Key;
            _idLookup.Add(key.Id, key);
            _nameLookup.Add(key.Name, key);
        }

        private void RemovePlayer(IPlayer player)
        {
            var key = player.Key;
            _idLookup.Remove(key.Id);
            _nameLookup.Remove(key.Name);
        }

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Misc.AssignNullAndDispose(ref _l);

                _isDisposed = true;
            }
        }

        #endregion
    }
}
