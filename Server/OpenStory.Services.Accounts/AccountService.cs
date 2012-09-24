using System;
using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Account
{
    /// <summary>
    /// Represents an in-memory account activity service.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    internal sealed class AccountService : GameServiceBase, IAccountService
    {
        private readonly Dictionary<int, ActiveAccount> activeAccounts;

        private readonly AtomicInteger currentSessionId;

        public AccountService()
        {
            this.activeAccounts = new Dictionary<int, ActiveAccount>(256);

            this.currentSessionId = new AtomicInteger(0);
        }

        #region IAccountService Members

        /// <inheritdoc />
        public bool TryRegisterSession(int accountId, out int sessionId)
        {
            if (this.activeAccounts.ContainsKey(accountId))
            {
                sessionId = 0;
                return false;
            }
            else
            {
                sessionId = this.currentSessionId.Increment();

                var account = new ActiveAccount(accountId, sessionId);
                this.activeAccounts.Add(accountId, account);
                return true;
            }
        }

        /// <inheritdoc />
        public bool TryRegisterCharacter(int accountId, int characterId)
        {
            ActiveAccount account;
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                return false;
            }
            else
            {
                account.RegisterCharacter(characterId);
                return true;
            }
        }

        /// <inheritdoc />
        public bool TryUnregisterSession(int accountId)
        {
            ActiveAccount account;
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                return false;
            }
            else
            {
                this.activeAccounts.Remove(accountId);
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
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                lag = default(TimeSpan);
                return false;
            }
            else
            {
                lag = account.KeepAlive();
                return true;
            }
        }

        #endregion

        #region Overrides of GameServiceBase

        public override bool Configure(ServiceConfiguration configuration, out string error)
        {
            error = null;
            return true;
        }

        #endregion
    }
}
