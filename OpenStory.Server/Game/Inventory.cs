using System;
using System.Collections.Generic;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a container for items.
    /// </summary>
    public abstract class ItemContainer<TItemCluster> 
        where TItemCluster : ItemCluster
    {
        /// <summary>
        /// Gets the maximum capacity for this container.
        /// </summary>
        public abstract int MaxCapacity { get; }

        /// <summary>
        /// Gets the current capacity of this container.
        /// </summary>
        public int SlotCapacity { get; protected set; }

        /// <summary>
        /// Gets the number of free slots in this container.
        /// </summary>
        public int FreeSlots
        {
            get { return this.SlotCapacity - this.slots.Count; }
        }

        private readonly Dictionary<int, TItemCluster> slots;

        /// <summary>
        /// Initializes a new ItemContainer instance with the specified slot capacity.
        /// </summary>
        /// <param name="slotCapacity">The slot capacity for this container.</param>
        protected ItemContainer(int slotCapacity)
        {
            this.SlotCapacity = slotCapacity;
            this.slots = new Dictionary<int, TItemCluster>(slotCapacity);
        }

        /// <summary>
        /// Expands the item container with the specified number of slots.
        /// </summary>
        /// <param name="count">The number of slots to add.</param>
        public void Expand(int count)
        {
            int newCapacity = this.SlotCapacity + count;
            if (newCapacity > this.MaxCapacity)
            {
                throw new ArgumentException("You cannot expand this container past its max capacity.", "count");
            }

            this.SlotCapacity = newCapacity;
        }
    }
}
