using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// The type of an inventory.
    /// </summary>
    [Serializable]
    public enum InventoryType
    {
        /// <summary>
        /// Default value.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The inventory contains equips.
        /// </summary>
        Equip = 1,

        /// <summary>
        /// The inventory contains useable items.
        /// </summary>
        Use = 2,

        /// <summary>
        /// The inventory contains setup items.
        /// </summary>
        Setup = 3,

        /// <summary>
        /// The inventory contains miscellaneous items.
        /// </summary>
        Etc = 4,

        /// <summary>
        /// The inventory contains Cash Shop items.
        /// </summary>
        Cash = 5,
    }
}