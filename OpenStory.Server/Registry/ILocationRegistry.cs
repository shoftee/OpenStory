using System;
using System.Collections.Generic;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides methods for querying and setting player locations.
    /// </summary>
    public interface ILocationRegistry
    {
        /// <summary>
        /// Gets a map of <see cref="PlayerLocation"/> instances for the given player identifiers.
        /// </summary>
        /// <remarks>
        /// This method will only return distinct results, that is, duplicates in the input sequence will be discarded.
        /// </remarks>
        /// <param name="playerIds">A <see cref="IEnumerable{Int32}"/> with the identifiers of the players to locate.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="playerIds"/> is <c>null</c>.
        /// </exception>
        /// <returns>
        /// a <see cref="Dictionary{Int32, PlayerLocation}"/> 
        /// mapping each input player identifier to a <see cref="PlayerLocation"/> 
        /// instance, or to <c>null</c> if the player was not located.
        /// </returns>
        Dictionary<int, PlayerLocation> GetLocationsForAll(IEnumerable<int> playerIds);

        /// <summary>
        /// Gets a <see cref="PlayerLocation"/> instance for the given player identifier.
        /// </summary>
        /// <param name="playerId">The identifier of the player to locate.</param>
        /// <returns>a <see cref="PlayerLocation"/> instance, or <c>null</c> if the player was not found.</returns>
        PlayerLocation GetLocation(int playerId);

        /// <summary>
        /// Sets the location of a player.
        /// </summary>
        /// <param name="playerId">The identifier of the player.</param>
        /// <param name="channelId">The identifier of the channel the player is currently in.</param>
        /// <param name="mapId">The identifier of the map the player is currently in.</param>
        void SetLocation(int playerId, int channelId, int mapId);

        /// <summary>
        /// Removes the specified player from location tracking.
        /// </summary>
        /// <param name="playerId">The identifier of the player.</param>
        void RemoveLocation(int playerId);
    }
}