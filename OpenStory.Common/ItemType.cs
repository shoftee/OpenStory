namespace OpenStory.Common
{
    /// <summary>
    /// The type of an item.
    /// </summary>
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
        /// and usuall does not have a unique identifier.
        /// </summary>
        Item = 2
    }
}