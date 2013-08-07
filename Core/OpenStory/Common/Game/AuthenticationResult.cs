using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// A response code for an authentication attempt.
    /// </summary>
    [Serializable]
    public enum AuthenticationResult
    {
        /// <summary>
        /// Client authenticated.
        /// </summary>
        [PacketValue(0x00)]
        Success,

        /// <summary>
        /// The account has been deleted or is blocked.
        /// </summary>
        [PacketValue(0x03)]
        AccountDeletedOrBlocked,

        /// <summary>
        /// Incorrect password.
        /// </summary>
        [PacketValue(0x04)]
        IncorrectPassword,

        /// <summary>
        /// The user name is not registered
        /// </summary>
        [PacketValue(0x05)]
        NotRegistered,

        /// <summary>
        /// The account already has another session running.
        /// </summary>
        [PacketValue(0x07)]
        AlreadyLoggedIn,

        /// <summary>
        /// The server has too many active connections.
        /// </summary>
        [PacketValue(0x0A)]
        TooManyConnections,

        /// <summary>
        /// This is the first time the account is logged into, show the License Agreement.
        /// </summary>
        [PacketValue(0x17)]
        FirstRun,
    }
}
