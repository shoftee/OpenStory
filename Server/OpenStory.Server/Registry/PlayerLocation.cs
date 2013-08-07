using System;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Holds location information for a player.
    /// </summary>
    [Serializable]
    public sealed class PlayerLocation : IEquatable<PlayerLocation>
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
        /// Initializes a new instance of the <see cref="PlayerLocation"/> class.
        /// </summary>
        /// <param name="channelId">The ID of the Channel.</param>
        /// <param name="mapId">The ID of the Map.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="channelId"/> or <paramref name="mapId"/> are negative.
        /// </exception>
        internal PlayerLocation(int channelId, int mapId)
        {
            if (channelId < 0)
            {
                throw new ArgumentOutOfRangeException("channelId", channelId, Framework.Model.Common.Exceptions.ChannelIdMustBeNonNegative);
            }

            if (mapId < 0)
            {
                throw new ArgumentOutOfRangeException("mapId", mapId, Framework.Model.Common.Exceptions.MapIdMustBeNonNegative);
            }

            this.ChannelId = channelId;
            this.MapId = mapId;
        }

        /// <inheritdoc />
        public bool Equals(PlayerLocation other)
        {
            if (other == null)
            {
                return false;
            }

            return this.EqualsInternal(other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as PlayerLocation;
            if (other == null)
            {
                return false;
            }

            return this.EqualsInternal(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return unchecked((this.ChannelId * 397) ^ this.MapId);
        }

        private bool EqualsInternal(PlayerLocation other)
        {
            return this.ChannelId.Equals(other.ChannelId)
                   && this.MapId.Equals(other.MapId);
        }

        /// <summary>
        /// Checks if two <see cref="PlayerLocation"/> objects are equal.
        /// </summary>
        /// <param name="location1">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <param name="location2">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <returns><see langword="true"/> if the two objects are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(PlayerLocation location1, PlayerLocation location2)
        {
            return object.Equals(location1, location2);
        }

        /// <summary>
        /// Checks if two <see cref="PlayerLocation"/> objects are not equal.
        /// </summary>
        /// <param name="location1">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <param name="location2">A <see cref="PlayerLocation"/> object to compare.</param>
        /// <returns><see langword="true"/> if the two objects are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(PlayerLocation location1, PlayerLocation location2)
        {
            return !(location1 == location2);
        }
    }
}
