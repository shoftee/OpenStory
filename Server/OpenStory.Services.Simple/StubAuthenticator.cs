using System;
using OpenStory.Common.Game;
using OpenStory.Common.IO;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Auth;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Simple
{
    internal sealed class StubAuthenticator : IAuthenticator
    {
        public AuthenticationResult Authenticate(IUnsafePacketReader credentialsReader, out IAccountSession session, out Account account)
        {
            session = null;
            account = null;

            var username = credentialsReader.ReadLengthString();
            if (username != "admin")
            {
                return AuthenticationResult.NotRegistered;
            }

            var password = credentialsReader.ReadLengthString();
            if (password != "admin")
            {
                return AuthenticationResult.IncorrectPassword;
            }

            session = new StubAccountSession();
            account = new Account()
                      {
                          AccountId = 1,
                          UserName = "admin",
                          Password = "admin",
                          Gender = Gender.Male,
                          GameMasterLevel = GameMasterLevel.GameMaster,
                          Status = AccountStatus.Active,
                          AccountPin = "0000",
                      };
            return AuthenticationResult.Success;
        }

        private sealed class StubAccountSession : IAccountSession
        {
            public int SessionId { get; }
            public int AccountId { get; }
            public string AccountName { get; }

            public StubAccountSession()
            {
                AccountId = 1;
                AccountName = "admin";
                SessionId = 1;
            }
            public bool TryKeepAlive(out TimeSpan lag)
            {
                lag = TimeSpan.FromMilliseconds(16);
                return true;
            }

            public void Dispose()
            {
            }
        }
    }
}
