using System;
using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Account
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    class AccountService : IAccountService
    {
        private readonly Dictionary<int, ActiveAccount> activeAccounts;

        private readonly AtomicInteger currentSessionId;

        public AccountService()
        {
            this.activeAccounts = new Dictionary<int, ActiveAccount>(256);

            this.currentSessionId = new AtomicInteger(0);
        }

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
                sessionId = currentSessionId.Increment();

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

        #region Implementation of IGameService

        /// <inheritdoc />
        public void Start()
        {
            throw new InvalidOperationException("The AccountService is always running.");
        }

        /// <inheritdoc />
        public void Stop()
        {
            throw new InvalidOperationException("The AccountService is always running.");
        }

        /// <inheritdoc />
        public bool Ping()
        {
            return true;
        }

        #endregion
    }
}