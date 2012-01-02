using System;
using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Common;
using OpenStory.ServiceModel;

namespace OpenStory.AccountService
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

        public bool IsActive(int accountId)
        {
            return this.activeAccounts.ContainsKey(accountId);
        }

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

        public void RegisterCharacter(int accountId, int characterId)
        {
            ActiveAccount account;
            if (!this.activeAccounts.TryGetValue(accountId, out account))
            {
                throw new InvalidOperationException("The specified account is not active.");
            }
            account.RegisterCharacter(characterId);
        }

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

        public void Start()
        {
            throw new InvalidOperationException("The AccountService is always running.");
        }

        public void Stop()
        {
            throw new InvalidOperationException("The AccountService is always running.");
        }

        public bool Ping()
        {
            return true;
        }

        #endregion
    }
}