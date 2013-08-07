using OpenStory.Framework.Model.Common;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for data operations with accounts.
    /// </summary>
    public interface IAccountProvider
    {
        /// <summary>
        /// Retrieves an instance of <see cref="Account"/> for the account with the specified user name.
        /// </summary>
        /// <param name="userName">The user name of the account.</param>
        /// <returns>an instance of <see cref="Account"/>, or <see langword="null"/> if no account was found.</returns>
        Account LoadByUserName(string userName);

        /// <summary>
        /// Saves the provided account object.
        /// </summary>
        /// <param name="account">The account to save.</param>
        void Save(Account account);
    }
}
