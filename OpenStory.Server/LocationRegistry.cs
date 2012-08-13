using System.Collections.Generic;
using System.Linq;

namespace OpenStory.Server
{
    class LocationRegistry
    {
        private readonly Dictionary<int, PlayerLocation> locations;

        /// <summary>
        /// Gets a <see cref="PlayerLocation"/> instance for the given player ID.
        /// </summary>
        /// <param name="playerId">The ID of the player to locate.</param>
        /// <returns>
        /// a <see cref="PlayerLocation"/> instance, or <c>null</c> if the player was not found.
        /// </returns>
        public PlayerLocation this[int playerId]
        {
            get { return this.GetLocation(playerId); }
        }

        /// <summary>
        /// Gets a map of <see cref="PlayerLocation"/> instances for the given player IDs.
        /// </summary>
        /// <param name="playerIds">A <see cref="IEnumerable{Int32}"/> with the IDs of the players to locate.</param>
        /// <returns>
        /// <para>
        /// A <see cref="Dictionary{Int32, PlayerLocation}"/> 
        /// mapping each input player ID to a <see cref="PlayerLocation"/> 
        /// instance, or to <c>null</c> if the player was not located.
        /// </para>
        /// </returns>
        public Dictionary<int, PlayerLocation> this[IEnumerable<int> playerIds]
        {
            get { return this.GetLocationsForAll(playerIds); }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LocationRegistry"/>.
        /// </summary>
        public LocationRegistry()
        {
            this.locations = new Dictionary<int, PlayerLocation>();
        }

        /// <summary>
        /// Gets a map of <see cref="PlayerLocation"/> instances for the given player IDs.
        /// </summary>
        /// <remarks>
        /// This method will only return distinct results, that is, duplicates in the input sequence will be discarded.
        /// </remarks>
        /// <param name="playerIds">A <see cref="IEnumerable{Int32}"/> with the IDs of the players to locate.</param>
        /// <returns><para>
        /// A <see cref="Dictionary{Int32, PlayerLocation}"/> 
        /// mapping each input player ID to a <see cref="PlayerLocation"/> 
        /// instance, or to <c>null</c> if the player was not located.
        /// </para></returns>
        public Dictionary<int, PlayerLocation> GetLocationsForAll(IEnumerable<int> playerIds)
        {
            return playerIds
                .Distinct()
                .ToDictionary(playerId => playerId, this.GetLocation);
        }

        /// <summary>
        /// Gets a <see cref="PlayerLocation"/> instance for the given player ID.
        /// </summary>
        /// <param name="playerId">The ID of the player to locate.</param>
        /// <returns>a <see cref="PlayerLocation"/> instance, or <c>null</c> if the player was not found.</returns>
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

        /// <summary>
        /// Sets the location of a player.
        /// </summary>
        /// <param name="playerId">The ID of the player.</param>
        /// <param name="channelId">The ID of the channel the player is currently in.</param>
        /// <param name="mapId">The ID of the map the player is currently in.</param>
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

        /// <summary>
        /// Removes the specified player from location tracking.
        /// </summary>
        /// <param name="playerId">The ID of the player.</param>
        public void RemoveLocation(int playerId)
        {
            this.locations.Remove(playerId);
        }
    }
}
