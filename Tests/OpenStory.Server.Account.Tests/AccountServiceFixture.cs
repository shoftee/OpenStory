using System;
using FluentAssertions;
using NodaTime;
using NUnit.Framework;
using OpenStory.Server.Accounts;
using OpenStory.Services.Account;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Account.Tests
{
    [Category("OpenStory.Services.Account")]
    [TestFixture]
    public class AccountServiceFixture
    {
        #region TestClock

        class TestClock : IClock
        {
            public Instant Now { get; set; }

            public TestClock()
            {
                this.Now = SystemClock.Instance.Now;
            }
        }

        private static IAccountService CreateAccountService()
        {
            return new AccountService(SystemClock.Instance);
        }

        private static IAccountService CreateAccountService(IClock clock)
        {
            return new AccountService(clock);
        }

        #endregion

        #region TryRegisterSession(int accountId, out int sessionId)

        [Test]
        public void TryRegisterSession_Should_Succeed_And_Provide_NonZero_SessionId()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            int sessionId;
            bool success = service.TryRegisterSession(AccountId, out sessionId);

            success.Should().BeTrue();
            sessionId.Should().NotBe(0);
        }

        [Test]
        public void TryRegisterSession_Should_Fail_For_Already_Active_Account()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);
            bool success = service.TryRegisterSession(AccountId, out sessionId);

            success.Should().BeFalse();
        }

        [Test]
        public void TryRegisterSession_Should_Succeed_After_Previous_Session_Is_Unregistered()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);
            service.TryUnregisterSession(AccountId);

            bool success = service.TryRegisterSession(AccountId, out sessionId);

            success.Should().BeTrue();
            sessionId.Should().NotBe(0);
        }

        #endregion

        #region TryUnregisterSession(int accountId)

        [Test]
        public void TryUnregisterSession_Should_Fail_For_Inactive_Account()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            bool success = service.TryUnregisterSession(AccountId);

            success.Should().BeFalse();
        }

        [Test]
        public void TryUnregisterSession_Should_Succeed_For_Active_Account()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);
            bool success = service.TryUnregisterSession(AccountId);

            success.Should().BeTrue();
        }

        [Test]
        public void TryUnregisterSession_Should_Fail_For_No_Longer_Active_Account()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);
            service.TryUnregisterSession(AccountId);
            bool success = service.TryUnregisterSession(AccountId);

            success.Should().BeFalse();
        }

        #endregion

        #region TryRegisterCharacter(int accountId, int characterId)

        [Test]
        public void TryRegisterCharacter_Should_Fail_For_Inactive_Account()
        {
            const int AccountId = 1, CharacterId = 1337;
            var service = CreateAccountService();

            bool success = service.TryRegisterCharacter(AccountId, CharacterId);

            success.Should().BeFalse();
        }

        [Test]
        public void TryRegisterCharacter_Should_Fail_For_No_Longer_Active_Account()
        {
            const int AccountId = 1, CharacterId = 1337;
            var service = CreateAccountService();
            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);
            service.TryUnregisterSession(AccountId);

            bool success = service.TryRegisterCharacter(AccountId, CharacterId);

            success.Should().BeFalse();
        }

        [Test]
        public void TryRegisterCharacter_Should_Succeed_For_Active_Account()
        {
            const int AccountId = 1, CharacterId = 1337;
            var service = CreateAccountService();
            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);

            bool success = service.TryRegisterCharacter(AccountId, CharacterId);

            success.Should().BeTrue();
        }

        [Test]
        public void TryRegisterCharacter_Should_Fail_For_Account_With_Already_Registered_Character()
        {
            const int AccountId = 1, CharacterId = 1337;
            var service = CreateAccountService();
            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);
            service.TryRegisterCharacter(AccountId, CharacterId);

            bool success = service.TryRegisterCharacter(AccountId, CharacterId);

            success.Should().BeFalse();
        }

        #endregion

        #region TryKeepAlive(int accountId, out TimeSpan lag)

        [Test]
        public void TryKeepAlive_Should_Fail_For_Inactive_Account()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            TimeSpan lag;
            bool success = service.TryKeepAlive(AccountId, out lag);

            success.Should().BeFalse();
        }

        [Test]
        public void TryKeepAlive_Should_Succeed_For_Active_Account()
        {
            const int AccountId = 1;
            var service = CreateAccountService();

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);

            TimeSpan lag;
            bool success = service.TryKeepAlive(AccountId, out lag);

            success.Should().BeTrue();
        }

        [Test]
        public void TryRegisterSession_Should_Initialize_Session_KeepAlive_Correctly()
        {
            const int AccountId = 1;
            var clock = new TestClock();
            var service = CreateAccountService(clock);

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);

            TimeSpan lag;
            service.TryKeepAlive(AccountId, out lag);

            lag.Duration().Should().Be(TimeSpan.Zero);
        }

        [Test]
        public void TryKeepAlive_Should_Return_Correct_Lag_Value()
        {
            const int AccountId = 1;
            var clock = new TestClock();
            var service = CreateAccountService(clock);

            int sessionId;
            service.TryRegisterSession(AccountId, out sessionId);

            TimeSpan lag;
            service.TryKeepAlive(AccountId, out lag);
            clock.Now = clock.Now.Plus(Duration.FromSeconds(15));
            service.TryKeepAlive(AccountId, out lag);

            lag.Duration().Should().Be(TimeSpan.FromSeconds(15));
        }

        #endregion
    }
}
