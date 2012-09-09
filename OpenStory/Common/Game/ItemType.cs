using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// The type of an item.
    /// </summary>
    [Serializable]
    public enum ItemType
    {
        /// <summary>
        /// Default value.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The item is an equip. It does not stack with 
        /// other items and has a unique identifier.
        /// </summary>
        Equip = 1,

        /// <summary>
        /// The item is generic. It usually stacks with other items 
        /// and usually does not have a unique identifier.
        /// </summary>
        Item = 2,

        /// <summary>
        /// This item is a pet. It does not stack with other items and it has a pet identifier.
        /// </summary>
        Pet = 3
    }
}
