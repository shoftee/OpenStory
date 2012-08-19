using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Data;
using OpenStory.Server.Data.Providers;
using OpenStory.Server.Modules;

namespace OpenStory.Server
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Bootstrap
    {
        private static readonly Bootstrap instance = new Bootstrap();

        /// <summary>
        /// 
        /// </summary>
        public static DataModule Data { get { return instance.data; } }

        private readonly DataModule data;

        private Bootstrap()
        {
            data = new ModuleBuilder<DataModule>()
                .Register("Bans", new BanDataProvider())
                .Register("Accounts", new AccountDataProvider())
                .BuildAndReset();
        }

        private class BanDataProvider : IBanDataProvider
        {
            public bool BanByAccountId(int accountId, string reason, DateTimeOffset? expiration = null)
            {
                // TODO
                return false;
            }
        }

        private class AccountDataProvider : IAccountDataProvider
        {
            public Account LoadByUserName(string userName)
            {
                // TODO
                return null;
            }

            public void Save(Account account)
            {
                // TODO
                return;
            }
        }

    }
}
