using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// The gender of a user.
    /// </summary>
    [Serializable]
    public enum Gender
    {
        /// <summary>
        /// Male gender.
        /// </summary>
        Male = 0,

        /// <summary>
        /// Female gender.
        /// </summary>
        Female = 1,

        /// <summary>
        /// Unspecified gender.
        /// </summary>
        Unspecified = 2
    }
}
