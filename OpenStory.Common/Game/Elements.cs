using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// Elemental attributes for a game skill.
    /// </summary>
    [Serializable]
    [Flags]
    public enum Elements
    {
        /// <summary>
        /// Default value.
        /// </summary>
        None = 0,

        /// <summary>
        /// The fire attribute.
        /// </summary>
        /// <remarks>The associated character is <c>F</c>.</remarks>
        Fire = 0x1, // F

        /// <summary>
        /// The ice attribute.
        /// </summary>
        /// <remarks>The associated character is <c>I</c>.</remarks>
        Ice = 0x2,

        /// <summary>
        /// The lightning attribute.
        /// </summary>
        /// <remarks>The associated character is <c>L</c>.</remarks>
        Lightning = 0x4,

        /// <summary>
        /// The poison attribute.
        /// </summary>
        /// <remarks>The associated character is <c>S</c>.</remarks>
        Poison = 0x8,

        /// <summary>
        /// The holy attribute.
        /// </summary>
        /// <remarks>The associated character is <c>H</c>.</remarks>
        Holy = 0x10,

        /// <summary>
        /// The dark attribute.
        /// </summary>
        /// <remarks>The associated character is <c>D</c>.</remarks>
        Dark = 0x20,

        /// <summary>
        /// No elemental attributes, a physical attack.
        /// </summary>
        /// <remarks>The associated character is <c>P</c>.</remarks>
        Physical = 0x40,
    }
}