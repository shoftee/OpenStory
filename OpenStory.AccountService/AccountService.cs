using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Server;
using OpenStory.Server.Data;

namespace OpenStory.AccountService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class AccountService : IAccountService, ISessionManager
    {
        private readonly Dictionary<int, AccountSession> sessions;

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
            return session;
        }

        class AccountSession : IAccountSession
        {
            private ISessionManager parent;

            public int AccountId { get; private set; }

            public string AccountName { get; private set; }

            public AccountSession(ISessionManager parent, Account account)
            {
                this.AccountId = account.AccountId;
                this.AccountName = account.UserName;

                this.parent = parent;
            }

            public void Dispose()
            {
                parent.UnregisterSession(this.AccountId);
            }
        }

        public void UnregisterSession(int accountId)
        {
            this.sessions.Remove(accountId);
        }
    }
}