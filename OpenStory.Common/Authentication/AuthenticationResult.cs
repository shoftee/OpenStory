namespace OpenStory.Common.Authentication
{
    /// <summary>
    /// A response code for an authentication attempt.
    /// </summary>
    public enum AuthenticationResult
    {
        /// <summary>
        /// Client authenticated.
        /// </summary>
        Success = 0x00,
        /// <summary>
        /// The account has been deleted or is blocked.
        /// </summary>
        AccountDeletedOrBlocked = 0x03,
        /// <summary>
        /// Incorrect password.
        /// </summary>
        IncorrectPassword = 0x04,
        /// <summary>
        /// The user name is not registered
        /// </summary>
        NotRegistered = 0x05,
        /// <summary>
        /// The account already has another session running.
        /// </summary>
        AlreadyLoggedIn = 0x07,
        /// <summary>
        /// This is the first time the account is logged into, show the License Agreement.
        /// </summary>
        FirstRun = 0x17
    }
}