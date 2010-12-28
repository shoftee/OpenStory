using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Constants;
using OpenMaple.Cryptography;
using OpenMaple.Data;

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

        public AccountSession(Account account)
        {
            if (account.AccountId == -1)
            {
                throw new ArgumentException("You must provide a valid account.", "account");
            }
            this.AccountId = account.AccountId;
            this.UserName = account.UserName;
            this.GameMasterLevel = account.GameMasterLevel;
        }
    }
}
