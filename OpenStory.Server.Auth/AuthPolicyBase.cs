using OpenStory.Server.Data;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    internal abstract class AuthPolicyBase
    {
        private readonly IAccountService accountService;

        protected IAccountService AccountService { get { return this.accountService; } }

        protected AuthPolicyBase(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        private sealed class AccountSession : IAccountSession
        {
            private readonly IAccountService parent;

            /// <inheritdoc />
            public int SessionId { get; private set; }

            /// <inheritdoc />
            public int AccountId { get; private set; }

            /// <inheritdoc />
            public string AccountName { get; private set; }

            /// <summary>
            /// Initializes a new instance of <see cref="AccountSession"/>.
            /// </summary>
            /// <param name="parent">The <see cref="IAccountService"/> managing this session.</param>
            /// <param name="sessionId">The session identifier.</param>
            /// <param name="data">The loaded session data.</param>
            public AccountSession(IAccountService parent, int sessionId, Account data)
            {
                this.SessionId = sessionId;
                this.AccountId = data.AccountId;
                this.AccountName = data.UserName;

                this.parent = parent;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                parent.TryUnregisterSession(this.AccountId);
            }
        }

        /// <summary>
        /// Provides an <see cref="IAccountSession"/> with the specified properties.
        /// </summary>
        /// <param name="parent">The account service handling this session.</param>
        /// <param name="sessionId">The account session ID.</param>
        /// <param name="data">The account data for this session.</param>
        /// <returns>a reference to the constructed <see cref="IAccountSession"/>.</returns>
        protected static IAccountSession GetSession(IAccountService parent, int sessionId, Account data)
        {
            return new AccountSession(parent, sessionId, data);
        }
    }
}