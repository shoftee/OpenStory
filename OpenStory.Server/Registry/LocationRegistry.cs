using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// A registry for player locations.
    /// </summary>
    public sealed class LocationRegistry : ILocationRegistry
    {
        private readonly Dictionary<int, PlayerLocation> locations;

        /// <summary>
        /// Initializes a new instance of <see cref="LocationRegistry"/>.
        /// </summary>
        public LocationRegistry()
        {
            this.locations = new Dictionary<int, PlayerLocation>();
        }

        /// <inheritdoc />
        public Dictionary<int, PlayerLocation> GetLocationsForAll(IEnumerable<int> playerIds)
        {
            if (playerIds == null)
            {
                throw new ArgumentNullException("playerIds");
            }

            return playerIds
                .Distinct()
                .ToDictionary(playerId => playerId, this.GetLocation);
        }

        /// <inheritdoc />
        public PlayerLocation GetLocation(int playerId)
        {
            PlayerLocation location;
            if (this.locations.TryGetValue(playerId, out location))
            {
                return location;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc />
        public void SetLocation(int playerId, int channelId, int mapId)
        {
            var location = new PlayerLocation(channelId, mapId);
            if (this.locations.ContainsKey(playerId))
            {
                this.locations[playerId] = location;
            }
            else
            {
                this.locations.Add(playerId, location);
            }
        }

        /// <inheritdoc />
        public void RemoveLocation(int playerId)
        {
            this.locations.Remove(playerId);
        }
    }
}
