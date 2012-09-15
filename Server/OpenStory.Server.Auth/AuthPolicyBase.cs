using System;
using OpenStory.Server.Data;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    internal abstract class AuthPolicyBase
    {
        protected AuthPolicyBase()
        {
        }

        private sealed class AccountSession : IAccountSession
        {
            private readonly IAccountService service;

            /// <inheritdoc />
            public int SessionId { get; private set; }

            /// <inheritdoc />
            public int AccountId { get; private set; }

            /// <inheritdoc />
            public string AccountName { get; private set; }

            /// <summary>
            /// Initializes a new instance of <see cref="AccountSession"/>.
            /// </summary>
            /// <param name="service">The <see cref="IAccountService"/> managing this session.</param>
            /// <param name="sessionId">The session identifier.</param>
            /// <param name="data">The loaded session data.</param>
            public AccountSession(IAccountService service, int sessionId, Account data)
            {
                this.SessionId = sessionId;
                this.AccountId = data.AccountId;
                this.AccountName = data.UserName;

                this.service = service;
            }

            public bool TryKeepAlive(out TimeSpan lag)
            {
                return this.service.TryKeepAlive(this.AccountId, out lag);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                this.service.TryUnregisterSession(this.AccountId);
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
