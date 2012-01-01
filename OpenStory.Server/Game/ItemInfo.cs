using System;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents information about a game item.
    /// </summary>
    public class ItemInfo : IEquatable<ItemInfo>
    {
        /// <summary>
        /// Gets the identifier for this prototype.
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// Gets how many items of this prototype you can put in one cluster.
        /// </summary>
        public virtual int ClusterCapacity { get; private set; }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="ItemInfo"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="ItemInfo"/>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="ItemInfo"/>. </param><filterpriority>2</filterpriority>
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

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="ItemInfo"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.ItemId;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
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