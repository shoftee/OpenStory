using System;
using OpenStory.Framework.Model.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Simple implementation of <see cref="IAccountSession"/>
    /// </summary>
    internal sealed class AccountSession : IAccountSession
    {
        private readonly IAccountService service;

        /// <inheritdoc />
        public int SessionId { get; }

        /// <inheritdoc />
        public int AccountId { get; }

        /// <inheritdoc />
        public string AccountName { get; }

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
