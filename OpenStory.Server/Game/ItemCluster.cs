using System;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a stack of identical items.
    /// </summary>
    /// <typeparam name="TItemInfo">The <see cref="ItemInfo"/> type of the items in this cluster.</typeparam>
    public class ItemCluster<TItemInfo>
        where TItemInfo : ItemInfo
    {
        /// <summary>
        /// Gets the prototype of the items in this cluster.
        /// </summary>
        public TItemInfo Prototype { get; private set; }

        /// <summary>
        /// Gets whether the cluster is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Quantity == 0; }
        }

        /// <summary>
        /// Gets the number of items in this cluster.
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        /// Gets the identifier for the prototype of this cluster.
        /// </summary>
        public int ItemId
        {
            get { return this.Prototype.ItemId; }
        }

        /// <summary>
        /// Gets the item capacity of this cluster.
        /// </summary>
        public int ClusterCapacity
        {
            get { return this.Prototype.ClusterCapacity; }
        }

        /// <summary>
        /// Attempts to merge the specified ItemCluster with the current.
        /// </summary>
        /// <param name="other">The ItemCluster to merge.</param>
        /// <returns>
        /// the number of items that were carried over to the current cluster.
        /// </returns>
        public int Merge(ItemCluster<TItemInfo> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            // Note: This is actually not quite necessary, 
            // since Prototypes are immutable and only supplied from the cache, 
            // we could go with just identity check.
            if (!this.Prototype.Equals(other.Prototype))
            {
                throw new ArgumentException("The specified ItemCluster has a different prototype.", "other");
            }

            int freeSpace = this.ClusterCapacity - this.Quantity;
            int availableQuantity = Math.Min(freeSpace, other.Quantity);
            this.Quantity += availableQuantity;
            other.Quantity -= availableQuantity;

            return availableQuantity;
        }

        /// <summary>
        /// Gets the number of items that can be merged from the specified cluster into the current.
        /// </summary>
        /// <param name="other">The cluster to merge from.</param>
        /// <returns>
        /// the number of items that can be merged; 
        /// <c>0</c> if the <paramref name="other"/> is incompatible with this cluster.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="other"/> is <c>null</c>.
        /// </exception>
        public bool CanMergeWith(ItemCluster<TItemInfo> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (!this.Prototype.Equals(other.Prototype))
            {
                return false;
            }

            if (this.Quantity == this.ClusterCapacity)
            {
                return false;
            }

            return true;
        }
    }
}