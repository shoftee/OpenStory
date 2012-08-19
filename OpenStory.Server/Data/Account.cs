using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using OpenStory.Common.Game;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Represents an account.
    /// </summary>
    [DataContract]
    public sealed class Account
    {
        /// <summary>
        /// Gets the Account ID.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int AccountId { get; private set; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the e-mail address for the account.
        /// </summary>
        public string EmailAddress { get; private set; }

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        public string PasswordHash { get; private set; }

        /// <summary>
        /// Gets the game master access level for the account.
        /// </summary>
        public GameMasterLevel GameMasterLevel { get; private set; }

        /// <summary>
        /// Gets the gender for the account.
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// Gets the account's status.
        /// </summary>
        public AccountStatus Status { get; set; }

        internal Account(IDataRecord record)
        {
            this.AccountId = (int) record["AccountId"];
            this.UserName = (string) record["UserName"];
            this.PasswordHash = (string) record["PasswordHash"];
            this.EmailAddress = (string) record["EmailAddress"];
            this.GameMasterLevel = (GameMasterLevel) record["GameMasterLevel"];
            this.Gender = (Gender) record["record"];
            this.Status = (AccountStatus) record["record"];
        }

        /// <summary>
        /// Attempts to load the account information for the given user name.
        /// </summary>
        /// <param name="userName">The user name to query.</param>
        /// <returns>true if the account information was loaded successfully; otherwise, false.</returns>
        public static Account LoadByUserName(string userName)
        {
            using (var query = new SqlCommand("SELECT * FROM Account WHERE UserName=@userName"))
            {
                query.Parameters.Add("@userName", SqlDbType.VarChar, 12).Value = userName;
                Account accountData = null;
                bool result = DbHelpers.InvokeForSingle(query, record => accountData = new Account(record));

                return result ? accountData : null;
            }
        }
    }
}