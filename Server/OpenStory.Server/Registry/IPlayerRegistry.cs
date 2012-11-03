using System.Collections.Generic;

namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides methods for managing player objects and their identity.
    /// </summary>
    public interface IPlayerRegistry
    {
        /// <summary>
        /// Adds a player to the registry.
        /// </summary>
        /// <param name="player">The player to add.</param>
        void RegisterPlayer(IPlayer player);

        /// <summary>
        /// Removes a player from the registry.
        /// </summary>
        /// <param name="player">The player to remove.</param>
        void UnregisterPlayer(IPlayer player);

        /// <summary>
        /// Gets a player by the character identifier.
        /// </summary>
        /// <param name="id">The character identifier.</param>
        /// <returns>the matched player instance, or <c>null</c> if there was no match.</returns>
        IPlayer GetById(int id);

        /// <summary>
        /// Gets a player by the character name.
        /// </summary>
        /// <param name="name">The character name.</param>
        /// <returns>the matched player instance, or <c>null</c> if there was no match.</returns>
        IPlayer GetByName(string name);

        /// <summary>
        /// Runs a scan on the player instances, while locking the registry.
        /// </summary>
        /// <remarks>
        /// <para>The objects in the instance list will not become unregistered during the scan.</para>
        /// <para>The scan will however block write operations on the registry.</para>
        /// </remarks>
        /// <param name="whitelist">A list of player identifiers to match.</param>
        IEnumerable<IPlayer> Scan(IEnumerable<int> whitelist);
    }
}