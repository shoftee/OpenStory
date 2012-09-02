using System;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// The type of a ban.
    /// </summary>
    [Serializable]
    public enum BanType
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The user is banned by their account ID. They will be able to access other accounts.
        /// </summary>
        AccountId = 1,

        /// <summary>
        /// The user is banned by their current IP address. They will be able to access the game if their IP address changes.
        /// </summary>
        IpAddress = 2,

        /// <summary>
        /// The user is banned by their physical device address. They will be able to access the game if they use another device.
        /// </summary>
        MacAddress = 3,

        /// <summary>
        /// The user is banned by their hard drive's Serial ID. They will be able to access the game from a machine with a different one.
        /// </summary>
        VolumeSerialId = 4
    }
}
