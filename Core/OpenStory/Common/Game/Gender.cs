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
        [PacketValue(0)]
        Male = 0,

        /// <summary>
        /// Female gender.
        /// </summary>
        [PacketValue(1)]
        Female = 1,

        /// <summary>
        /// Unspecified gender.
        /// </summary>
        [PacketValue(2)]
        Unspecified = 2
    }
}
