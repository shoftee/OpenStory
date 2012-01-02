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
        private readonly HashSet<int> activeAccounts;
        private readonly Dictionary<int, int> sessionAccounts;
        private readonly Dictionary<int, int> sessionCharacters;

        private readonly AtomicInteger currentSessionId;

        public AccountService()
        {
            this.activeAccounts = new HashSet<int>();
            this.sessionAccounts = new Dictionary<int, int>(256);
            this.sessionCharacters = new Dictionary<int, int>(256);
            this.currentSessionId = new AtomicInteger(0);
        }

        public bool IsActive(int accountId)
        {
            return this.activeAccounts.Contains(accountId);
        }

        public bool TryRegisterSession(int accountId, out int sessionId)
        {
            if (this.activeAccounts.Contains(accountId))
            {
                sessionId = 0;
                return false;
            }
            else
            {
                this.activeAccounts.Add(accountId);
                sessionId = currentSessionId.Increment();
                this.sessionAccounts.Add(accountId, sessionId);
                return true;
            }
        }

        public void RegisterCharacter(int sessionId, int characterId)
        {
            if (!this.sessionAccounts.ContainsKey(sessionId))
            {
                throw new InvalidOperationException("The specified session is not registered.");
            }
            if (this.sessionCharacters.ContainsKey(sessionId))
            {
                throw new InvalidOperationException("The specified session already has an active character registered.");
            }
            this.sessionCharacters.Add(sessionId, characterId);
        }

        public bool TryUnregisterSession(int sessionId)
        {
            int accountId;
            if (!this.sessionAccounts.TryGetValue(sessionId, out accountId))
            {
                return false;
            }
            else
            {
                this.sessionCharacters.Remove(sessionId);
                this.sessionAccounts.Remove(sessionId);
                this.activeAccounts.Remove(accountId);
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