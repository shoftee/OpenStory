using System.Data;
using System.Data.SqlClient;
using OpenStory.Common;

namespace OpenStory.Server.Data
{
    public class AccountData
    {
        // From the game server's perspective, the account data is read-only.
        // If I ever make another perspective for it, I'll make another class.-
        private AccountData() {}
        public int AccountId { get; private set; }
        public string UserName { get; private set; }
        public string PasswordHash { get; private set; }
        public string EmailAddress { get; private set; }

        // TODO: Not in the DB yet.   
        public GameMasterLevel GameMasterLevel { get; private set; }
        public Gender Gender { get; private set; }
        public AccountStatus Status { get; private set; }

        /// <summary>
        /// Attempts to load the account information for the given user name.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <returns>true if the account information was loaded successfully; otherwise, false.</returns>
        public static AccountData LoadByUserName(string userName)
        {
            using (var query = new SqlCommand("SELECT * FROM Account WHERE UserName=@userName"))
            {
                DbUtils.AddParameter(query, "@userName", SqlDbType.VarChar, 12, userName);
                var accountData = new AccountData();
                bool result = DbUtils.InvokeForSingle(query, accountData.ReadInfo);

                return result ? accountData : null;
            }
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
}