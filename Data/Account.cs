using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenMaple.Constants;
using OpenMaple.Tools;

namespace OpenMaple.Data
{
    class Account
    {
        // From the game server's perspective, the account data is read-only.
        // If I ever make another perspective for it, I'll make another class.-
        public int AccountId { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public string EmailAddress { get; private set; }
        
        // TODO: Not in the DB yet.   
        public GameMasterLevel GameMasterLevel { get; private set; }
        public Gender Gender { get; private set; }
        public AccountStatus Status { get; private set; }

        public Account()
        {
            this.AccountId = -1;
        }

        public Account(string userName) : this()
        {
            this.LoadByUserName(userName);
        }

        /// <summary>
        /// Attempts to load the account information for the given user name.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <returns>true if the account information was loaded successfully; otherwise, false.</returns>
        public bool LoadByUserName(string userName)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM Account WHERE UserName=@userName");
            query.AddParameter("@userName", SqlDbType.VarChar, 12, userName);

            return DbUtils.GetSingleRecord(query, ReadInfo);
        }

        private void ReadInfo(IDataRecord reader)
        {
            this.AccountId = (int) reader["AccountId"];
            this.UserName = (string) reader["UserName"];
            this.PasswordHash = (string) reader["PasswordHash"];
            this.EmailAddress = (string) reader["EmailAddress"];
            this.GameMasterLevel = (GameMasterLevel) (byte) reader["GameMasterLevel"];
            this.Gender = (Gender) (byte) reader["Gender"];
            this.Status = (AccountStatus) (byte) reader["Status"];
        }
    }

    enum GameMasterLevel : byte
    {
        None = 0,
        GameMasterHelper = 1,
        GameMaster = 2
    }

    enum AccountStatus : byte
    {
        FirstRun = 0,
        Active = 1,
        Blocked = 2,
        Deleted = 3
    }
}
