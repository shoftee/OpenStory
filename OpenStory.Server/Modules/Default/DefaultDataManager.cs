using System;
using OpenStory.Server.Data;
using OpenStory.Server.Data.Providers;

namespace OpenStory.Server.Modules.Default
{
    /// <summary>
    /// Represents the default data manager.
    /// </summary>
    /// <remarks>
    /// <para>This is a stub DataManager. You don't really wanna use it. You can use it as an example, though.</para>
    /// <para>If you wanna implement your own DataManager, derive from the <see cref="DataManager"/> class instead.</para>
    /// </remarks>
    internal sealed class DefaultDataManager : DataManager
    {
        public DefaultDataManager()
        {
            this.RegisterComponent("Bans", new SomeBanDataProvider());
            this.RegisterComponent("Accounts", new SomeAccountDataProvider());
        }

        #region Stub providers

        private sealed class SomeBanDataProvider : IBanDataProvider
        {
            public bool BanByAccountId(int accountId, string reason, DateTimeOffset? expiration = null)
            {
                // This is a stub. It will always return "false", meaning, the ban was not issued.
                return false;
            }
        }

        private sealed class SomeAccountDataProvider : IAccountDataProvider
        {
            public Account LoadByUserName(string userName)
            {
                // This is a stub. It always returns "null", meaning, account not found.
                return null;
            }

            public void Save(Account account)
            {
                // This is a stub. It won't do jack.
            }
        }

        #endregion
    }
}