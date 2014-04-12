using System;
using System.Data;
using System.Runtime.Serialization;
using OpenStory.Common.Game;

namespace OpenStory.Framework.Model.Common
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
        public int AccountId { get; set; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        /// <summary>
        /// Gets the password hash.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets the gender for the account.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Gets the account-bound PIN.
        /// </summary>
        public string AccountPin { get; set; }

        /// <summary>
        /// Gets the creation time of the account.
        /// </summary>
        public DateTimeOffset CreationTime
        {
            get { return DateTimeOffset.UtcNow; }
        }

        /// <summary>
        /// Gets the 'quiet ban' reason code.
        /// </summary>
        public byte QuietBanReason
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the 'quiet ban' time for the account.
        /// </summary>
        public DateTimeOffset QuietBanTime
        {
            get { return DateTimeOffset.MinValue; }
        }

        /// <summary>
        /// Gets the game master access level for the account.
        /// </summary>
        public GameMasterLevel GameMasterLevel { get; set; }

        /// <summary>
        /// Gets whether the account is a game master account.
        /// </summary>
        public bool IsGameMaster
        {
            get { return this.GameMasterLevel == GameMasterLevel.GameMaster; }
        }

        /// <summary>
        /// Gets whether the account is a game master helper account.
        /// </summary>
        public bool IsGameMasterHelper
        {
            get { return this.GameMasterLevel == GameMasterLevel.GameMasterHelper; }
        }

        /// <summary>
        /// Gets the account's status.
        /// </summary>
        public AccountStatus Status { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        public Account()
        {
        }
    }
}
