using System;
using OpenStory.Common.Game;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Fluent;

namespace OpenStory.Server.Auth.Policy
{
    internal sealed class SimpleAuthPolicy : AuthPolicyBase<SimpleCredentials>
    {
        private readonly IAccountProvider accountProvider;

        public SimpleAuthPolicy(IAccountProvider accountProvider)
        {
            this.accountProvider = accountProvider;
        }

        /// <inheritdoc />
        public override AuthenticationResult Authenticate(SimpleCredentials credentials, out IAccountSession session)
        {
            string accountName = credentials.AccountName;
            Account account = this.accountProvider.LoadByUserName(accountName);
            if (account == null)
            {
                return Misc.FailWithResult(out session, AuthenticationResult.NotRegistered);
            }

            string password = credentials.Password;
            string hash = LoginCrypto.GetMd5HashString(password, true);
            if (!string.Equals(hash, account.PasswordHash, StringComparison.Ordinal))
            {
                return Misc.FailWithResult(out session, AuthenticationResult.IncorrectPassword);
            }

            var service = OS.Svc().Accounts().Get();
            int sessionId;
            if (!service.TryRegisterSession(account.AccountId, out sessionId))
            {
                return Misc.FailWithResult(out session, AuthenticationResult.AlreadyLoggedIn);
            }

            session = SimpleAuthPolicy.GetSession(service, sessionId, account);
            return AuthenticationResult.Success;
        }
    }
}
