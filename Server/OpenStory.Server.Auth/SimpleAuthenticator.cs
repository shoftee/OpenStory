using System;
using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    internal sealed class SimpleAuthenticator : IAuthenticator
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IAccountService _accountService;

        public SimpleAuthenticator(IAccountProvider accountProvider, IAccountService accountService)
        {
            _accountProvider = accountProvider;
            _accountService = accountService;
        }

        /// <inheritdoc />
        public AuthenticationResult Authenticate(IUnsafePacketReader credentialsReader, out IAccountSession session, out Account account)
        {
            // Default value for failure scenarios:
            session = null;

            // TODO: user name validation, throw IllegalPacketException if not valid
            var userName = credentialsReader.ReadLengthString();
            // Attempt to load the account.
            account = _accountProvider.LoadByUserName(userName);
            if (account == null)
            {
                // Fail with 'NotRegistered' if no account matches.
                return AuthenticationResult.NotRegistered;
            }

            // TODO: password validation, throw IllegalPacketException if not valid
            var password = credentialsReader.ReadLengthString();

            string hash = LoginCrypto.GetMd5HashString(password, true);
            if (!string.Equals(hash, account.Password, StringComparison.Ordinal))
            {
                // Fail with 'IncorrectPassword' if password hash is bad.
                return AuthenticationResult.IncorrectPassword;
            }

            // TODO: read other stuff from packet

            int sessionId;
            if (!_accountService.TryRegisterSession(account.AccountId, out sessionId))
            {
                // Fail with 'AlreadyLoggedIn' if there is another session running on this account.
                return AuthenticationResult.AlreadyLoggedIn;
            }

            // Create the session.
            session = new AccountSession(_accountService, sessionId, account);
            return AuthenticationResult.Success;
        }
    }
}
