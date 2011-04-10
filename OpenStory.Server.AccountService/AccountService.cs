using System;
using System.Collections.Generic;
using OpenStory.Server.Communication;
using OpenStory.Server.Data;

namespace OpenStory.Server.AccountService
{
    class AccountService : GameService, IAccountService, ISessionManager
    {
        private Dictionary<int, AccountSession> sessions;

        public AccountService()
        {
            this.sessions = new Dictionary<int, AccountSession>(256);
        }

        public bool IsActive(int accountId)
        {
            return this.sessions.ContainsKey(accountId);
        }

        public IAccountSession RegisterSession(Account account)
        {
            AccountSession session = new AccountSession(this, account);
            this.sessions.Add(account.AccountId, session);

            return session;
        }

        public void UnregisterSession(int accountId)
        {
            this.sessions.Remove(accountId);
        }

        class AccountSession : MarshalByRefObject, IAccountSession
        {
            private ISessionManager parent;

            public int AccountId { get; private set; }

            public string AccountName { get; private set; }

            public AccountSession(ISessionManager parent, Account account)
            {
                if (parent == null) throw new ArgumentNullException("parent");
                if (account == null) throw new ArgumentNullException("account");

                this.parent = parent;

                this.AccountId = account.AccountId;
                this.AccountName = account.UserName;
            }

            public void Dispose()
            {
                this.parent.UnregisterSession(this.AccountId);
            }
        }
    }
}