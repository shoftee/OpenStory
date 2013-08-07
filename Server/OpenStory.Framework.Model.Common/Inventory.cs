namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Represents a strongly-typed game inventory.
    /// </summary>
    /// <typeparam name="TItemInfo">The <see cref="ItemInfo"/> type of the items in the inventory.</typeparam>
    public abstract class Inventory<TItemInfo> : ItemContainer<TItemInfo>
        where TItemInfo : ItemInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Inventory{TItemInfo}"/>.
        /// </summary>
        /// <param name="slotCapacity">The initial slot capacity.</param>
        /// <inheritdoc />
        protected Inventory(int slotCapacity)
            : base(slotCapacity)
        {
        }
    }
}
