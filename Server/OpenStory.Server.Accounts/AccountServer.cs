using System;
using System.Collections.Generic;
using OpenStory.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;
using NodaTime;

namespace OpenStory.Server.Accounts
{
    /// <summary>
    /// Represents an in-memory account activity server.
    /// </summary>
    public class AccountServer : GameServerBase, IAccountService
    {
        private readonly IClock _clock;

        private readonly Dictionary<int, ActiveAccount> _activeAccounts;
        private readonly AtomicInteger _currentSessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountServer"/> class.
        /// </summary>
        public AccountServer(IClock clock)
        {
            _clock = clock;

            _activeAccounts = new Dictionary<int, ActiveAccount>(256);
            _currentSessionId = new AtomicInteger(0);
        }

        #region IAccountService Members

        /// <inheritdoc />
        public bool TryRegisterSession(int accountId, out int sessionId)
        {
            if (_activeAccounts.ContainsKey(accountId))
            {
                sessionId = 0;
                return false;
            }
            else
            {
                sessionId = _currentSessionId.Increment();

                var account = new ActiveAccount(accountId, sessionId);
                account.KeepAlive(_clock.Now);

                _activeAccounts.Add(accountId, account);
                return true;
            }
        }

        /// <inheritdoc />
        public bool TryRegisterCharacter(int accountId, int characterId)
        {
            ActiveAccount account;
            if (!_activeAccounts.TryGetValue(accountId, out account))
            {
                return false;
            }
            else
            {
                if (!account.CharacterId.HasValue)
                {
                    account.RegisterCharacter(characterId);
                    return true;
                }

                return false;
            }
        }

        /// <inheritdoc />
        public bool TryUnregisterSession(int accountId)
        {
            ActiveAccount account;
            if (!_activeAccounts.TryGetValue(accountId, out account))
            {
                return false;
            }
            else
            {
                _activeAccounts.Remove(accountId);
                if (account.CharacterId.HasValue)
                {
                    account.UnregisterCharacter();
                }

                return true;
            }
        }

        /// <inheritdoc />
        public bool TryKeepAlive(int accountId, out TimeSpan lag)
        {
            ActiveAccount account;
            if (!_activeAccounts.TryGetValue(accountId, out account))
            {
                lag = default(TimeSpan);
                return false;
            }
            else
            {
                lag = account.KeepAlive(_clock.Now).ToTimeSpan();
                return true;
            }
        }

        #endregion
    }
}
