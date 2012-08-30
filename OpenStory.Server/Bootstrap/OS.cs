using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Server.Data;
using OpenStory.Server.Data.Providers;
using OpenStory.Server.Modules;

namespace OpenStory.Server.Bootstrap
{
    /// <summary>
    /// The bootstrap entry point!
    /// </summary>
    public sealed class OS
    {
        private static readonly OS Instance = new OS();

        private readonly DataManager data;

        /// <summary>
        /// 
        /// </summary>
        public static DataManager Data { get { return Instance.data; } }

        private OS()
        {
            data = new ManagerBuilder<DataManager>()
                .Register("Bans", new BanDataProvider())
                .Register("Accounts", new AccountDataProvider())
                .BuildAndReset();
        }

        #region Stub providers

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

        #endregion

    }
}
