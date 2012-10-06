using System;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents information about a game item.
    /// </summary>
    /// <remarks>
    /// This is the thing that should be loaded by your item information provider, whatever it is.
    /// </remarks>
    public abstract class ItemInfo : IEquatable<ItemInfo>
    {
        /// <summary>
        /// Gets the identifier for this prototype.
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// Gets how many items of this prototype you can put in one cluster.
        /// </summary>
        public int ClusterCapacity { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ItemInfo"/>.
        /// </summary>
        /// <param name="itemId">The identifier for the item.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="itemId"/> is non-positive.
        /// </exception>
        protected ItemInfo(int itemId)
        {
            if (itemId <= 0)
            {
                throw new ArgumentOutOfRangeException("itemId", itemId, Exceptions.ItemIdMustBePositive);
            }

            this.ItemId = itemId;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }

            return this.Equals(obj as ItemInfo);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.ItemId;
        }

        /// <inheritdoc />
        public bool Equals(ItemInfo other)
        {
            if (other == null)
            {
                return false;
            }

            return this.ItemId == other.ItemId;
        }
    }
}
