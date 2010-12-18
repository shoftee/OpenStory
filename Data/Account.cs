using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using OpenMaple.Tools;

namespace OpenMaple.Data
{
    class Account
    {
        // This data will be read-only for now.
        public int AccountId { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public string EmailAddress { get; private set; }

        public Account()
        {
            this.AccountId = -1;
        }

        public Account(int accountId) : this()
        {
            this.LoadByAccountId(accountId);
        }

        public Account(string userName) : this()
        {
            this.LoadByUserName(userName);
        }

        /// <summary>
        /// Attempts to load the account information for the given account ID.
        /// </summary>
        /// <param name="accountId">The account ID to query.</param>
        /// <returns>true if the account information was loaded successfully; otherwise, false.</returns>
        public bool LoadByAccountId(int accountId)
        {
            SqlCommand query = new SqlCommand("SELECT * FROM Account WHERE AccountId=@accountId");
            query.AddParameter("@accountId", SqlDbType.Int, accountId);

            return DbUtils.GetSingleRecord(query, ReadInfo);
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
        }

    }
}
