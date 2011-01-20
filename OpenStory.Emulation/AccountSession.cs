using System;
using OpenStory.Common;
using OpenStory.Server.Data;

namespace OpenStory.Emulation
{
    internal interface IAccount
    {
        int AccountId { get; }
        string UserName { get; }
        Gender Gender { get; }
        GameMasterLevel GameMasterLevel { get; }
    }

    internal class AccountSession : IAccount
    {
        public AccountSession(AccountData accountData)
        {
            if (accountData.AccountId == -1)
            {
                throw new ArgumentException("You must provide a valid account.", "accountData");
            }
            this.AccountId = accountData.AccountId;
            this.UserName = accountData.UserName;
            this.GameMasterLevel = accountData.GameMasterLevel;
        }

        #region IAccount Members

        public int AccountId { get; private set; }
        public string UserName { get; private set; }
        public Gender Gender { get; private set; }
        public GameMasterLevel GameMasterLevel { get; private set; }

        #endregion
    }
}