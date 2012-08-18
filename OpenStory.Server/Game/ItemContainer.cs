using System;
using System.Collections.Generic;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a container for items.
    /// </summary>
    public abstract class ItemContainer<TItemInfo>
        where TItemInfo : ItemInfo
    {
        /// <summary>
        /// Gets the maximum capacity for this container.
        /// </summary>
        public abstract int MaxCapacity { get; }

        /// <summary>
        /// Gets the current capacity of this container.
        /// </summary>
        public int SlotCapacity { get; private set; }

        /// <summary>
        /// Gets the number of free slots in this container.
        /// </summary>
        public int FreeSlots
        {
            get { return this.SlotCapacity - this.slots.Count; }
        }

        private readonly Dictionary<int, ItemCluster<TItemInfo>> slots;

        /// <summary>
        /// Initializes a new instance of <see cref="ItemContainer{TItemInfo}"/>.
        /// </summary>
        /// <param name="slotCapacity">The initial slot capacity.</param>
        protected ItemContainer(int slotCapacity)
        {
            if (slotCapacity < 0)
            {
                throw new ArgumentOutOfRangeException("slotCapacity", slotCapacity, "'slotCapacity' must be non-negative.");
            }
            this.SlotCapacity = slotCapacity;

            this.slots = new Dictionary<int, ItemCluster<TItemInfo>>(slotCapacity);
        }

        /// <summary>
        /// Expands the item container with the specified number of slots.
        /// </summary>
        /// <param name="count">The number of slots to add.</param>
        public void Expand(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count, "'count' must be non-negative.");
            }

            int newCapacity = this.SlotCapacity + count;
            if (newCapacity > this.MaxCapacity)
            {
                throw new ArgumentException("You cannot expand this container beyond its max capacity.", "count");
            }

            this.SlotCapacity = newCapacity;
        }
    }
}
