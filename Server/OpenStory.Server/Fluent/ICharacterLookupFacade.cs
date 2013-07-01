using System.ComponentModel;
using OpenStory.Framework.Model.Common;

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
        /// <param name="key">The CharacterKey of the player.</param>
        /// <returns>a <see cref="PlayerLocation"/> object, or <c>null</c> if the player was not found.</returns>
        PlayerLocation Location(CharacterKey key);

        /// <summary>
        /// Gets a CharacterKey for the specified numeric identifier.
        /// </summary>
        /// <param name="id">The identifier of the player.</param>
        /// <returns>the CharacterKey of the player, or <c>null</c> if the player was not found.</returns>
        CharacterKey Character(int id);

        /// <summary>
        /// Gets a CharacterKey for the specified in-game name.
        /// </summary>
        /// <param name="name">The name of the player's character.</param>
        /// <returns>the CharacterKey of the player, or <c>null</c> if the player was not found.</returns>
        CharacterKey Character(string name);
    }
}