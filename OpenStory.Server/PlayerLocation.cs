using System;

namespace OpenStory.Server
{
    /// <summary>
    /// Holds location information for a player.
    /// </summary>
    [Serializable]
    public class PlayerLocation : IEquatable<PlayerLocation>
    {
        /// <summary>
        /// Gets the ID of the channel the player is currently in.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Gets the ID of the map the player is currently in.
        /// </summary>
        public int MapId { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="PlayerLocation"/>.
        /// </summary>
        /// <param name="channelId">The ID of the Channel.</param>
        /// <param name="mapId">The ID of the Map.</param>
        internal PlayerLocation(int channelId, int mapId)
        {
            this.ChannelId = channelId;
            this.MapId = mapId;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(PlayerLocation other)
        {
            if (other == null) return false;
            return this.EqualsInternal(other);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="LocationRegistry"/>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="LocationRegistry"/>. </param>
        /// <filterpriority>2</filterpriority>
        /// <returns>
        /// <c>true</c> if the specified <see cref="T:System.Object"/> is equal to the current <see cref="LocationRegistry"/>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            PlayerLocation other = obj as PlayerLocation;
            if (other == null) return false;

            return this.EqualsInternal(other);
        }

        private bool EqualsInternal(PlayerLocation other)
        {
            return this.ChannelId.Equals(other.ChannelId)
                   && this.MapId.Equals(other.MapId);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="LocationRegistry"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.ChannelId.GetHashCode() ^ this.MapId.GetHashCode();
        }

        /// <summary>
        /// Checks if two <see cref="PlayerLocation"/> objects are equal.
        /// </summary>
        /// <param name="location1">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <param name="location2">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <returns><c>true</c> if the two objects are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(PlayerLocation location1, PlayerLocation location2)
        {
            return Equals(location1, location2);
        }

        /// <summary>
        /// Checks if two <see cref="PlayerLocation"/> objects are not equal.
        /// </summary>
        /// <param name="location1">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <param name="location2">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <returns><c>true</c> if the two objects are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(PlayerLocation location1, PlayerLocation location2)
        {
            return !(location1 == location2);
        }
    }
}