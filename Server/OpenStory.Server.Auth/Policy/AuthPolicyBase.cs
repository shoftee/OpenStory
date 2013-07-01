using System;
using OpenStory.Common.Game;
using OpenStory.Framework.Model.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth.Policy
{
    internal abstract class AuthPolicyBase<TCredentials> : IAuthPolicy<TCredentials>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPolicyBase{TCredentials}" /> class.
        /// </summary>
        protected AuthPolicyBase()
        {
        }

        /// <inheritdoc />
        public abstract AuthenticationResult Authenticate(TCredentials credentials, out IAccountSession session);

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
        
        /// <summary>
        /// A secret implementation of <see cref="IAccountSession"/>
        /// </summary>
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
            /// Initializes a new instance of the <see cref="AccountSession"/> class.
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

            /// <inheritdoc />
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
    }
}
