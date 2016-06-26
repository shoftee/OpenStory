using System;
using System.Collections.Generic;

namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Represents a container for item stacks.
    /// </summary>
    /// <remarks>
    /// This can be used for storage, herb pouches, player inventories, and other nifty stuff.
    /// </remarks>
    /// <typeparam name="TItemInfo">The <see cref="ItemInfo"/> type for the contained items.</typeparam>
    public abstract class ItemContainer<TItemInfo>
        where TItemInfo : ItemInfo
    {
        private readonly Dictionary<int, ItemCluster<TItemInfo>> slots;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainer{TItemInfo}"/> class.
        /// </summary>
        /// <param name="slotCapacity">The initial slot capacity.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="slotCapacity"/> is negative.
        /// </exception>
        protected ItemContainer(int slotCapacity)
        {
            if (slotCapacity < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(slotCapacity), slotCapacity, CommonStrings.CapacityMustBeNonNegative);
            }

            this.SlotCapacity = slotCapacity;

            this.slots = new Dictionary<int, ItemCluster<TItemInfo>>(slotCapacity);
        }

        /// <summary>
        /// Expands the item container with the specified number of slots.
        /// </summary>
        /// <param name="count">The number of slots to add.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is negative.</exception>
        /// <exception cref="ArgumentException">Thrown if the expansion would cause the inventory to expand over its maximum capacity.</exception>
        public void Expand(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count), count, CommonStrings.CountMustBeNonNegative);
            }

            int newCapacity = this.SlotCapacity + count;
            if (newCapacity > this.MaxCapacity)
            {
                throw new ArgumentException(ModelStrings.CannotExpandBeyondMaxCapacity, nameof(count));
            }

            this.SlotCapacity = newCapacity;
        }
    }
}
