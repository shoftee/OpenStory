using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Cryptography;
using OpenMaple.Data;
using OpenMaple.Game;

namespace OpenMaple.Server
{
    interface IAccount {
        int AccountId { get; }
        string UserName { get; }
        Gender Gender { get; }
        GameMasterLevel GameMasterLevel { get; }
    }

    class AccountSession : IAccount
    {
        public int AccountId { get; private set; }
        public string UserName { get; private set; }
        public Gender Gender { get; private set; }
        public GameMasterLevel GameMasterLevel { get; private set; }

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
    }
}
