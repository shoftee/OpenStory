using System;
using OpenStory.Common.Auth;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Server.Bootstrap;
using OpenStory.Server.Data;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    internal sealed class SimpleAuthPolicy : AuthPolicyBase, IAuthPolicy<SimpleCredentials>
    {
        public SimpleAuthPolicy(IAccountService accountService)
            : base(accountService)
        {
        }

        /// <inheritdoc />
        public AuthenticationResult Authenticate(SimpleCredentials credentials, out IAccountSession session)
        {
            string accountName = credentials.AccountName;
            Account account = OS.Data.Accounts.LoadByUserName(accountName);
            if (account == null)
            {
                return MiscTools.FailWithResult(out session, AuthenticationResult.NotRegistered);
            }

            string password = credentials.Password;
            string hash = LoginCrypto.GetMd5HashString(password, true);
            if (!String.Equals(hash, account.PasswordHash, StringComparison.Ordinal))
            {
                return MiscTools.FailWithResult(out session, AuthenticationResult.IncorrectPassword);
            }

            var service = base.AccountService;
            int sessionId;
            if (!service.TryRegisterSession(account.AccountId, out sessionId))
            {
                return MiscTools.FailWithResult(out session, AuthenticationResult.AlreadyLoggedIn);
            }

            session = GetSession(service, sessionId, account);
            return AuthenticationResult.Success;
        }
    }
}
