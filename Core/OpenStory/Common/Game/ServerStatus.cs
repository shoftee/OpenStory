using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// World status options.
    /// </summary>
    [Serializable]
    public enum ServerStatus
    {
        /// <summary>
        /// The world is running normally and accepting players.
        /// </summary>
        Normal = 0,

        /// <summary>
        /// The world is very populated and may run slower than normally.
        /// </summary>
        HighlyPopulated = 1,

        /// <summary>
        /// The world is full and is not accepting more players.
        /// </summary>
        Full = 2
    }
}
