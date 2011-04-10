using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// The gender of a user.
    /// </summary>
    [Serializable]
    public enum Gender : byte
    {
        /// <summary>
        /// The user is male.
        /// </summary>
        Male = 0,
        /// <summary>
        /// The user is female.
        /// </summary>
        Female = 1
    }
}