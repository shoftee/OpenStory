using System.ComponentModel;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for character data lookup.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ICharacterLookupFacade
    {
        /// <summary>
        /// Gets the location of a player.
        /// </summary>
        /// <param name="id">The identifier of the player.</param>
        /// <returns>a <see cref="PlayerLocation"/> object, or <c>null</c> if the player was not found.</returns>
        PlayerLocation Location(int id);

        /// <summary>
        /// Gets the character name for a player.
        /// </summary>
        /// <param name="id">The identifier of the player.</param>
        /// <returns>the name of the player, or <c>null</c> if the player was not found.</returns>
        string Name(int id);

        /// <summary>
        /// Gets the identifer for a player.
        /// </summary>
        /// <param name="name">The name of the player's character.</param>
        /// <returns>the identifier of the player, or <c>null</c> if the player was not found.</returns>
        int? Id(string name);
    }
}