using System.Collections.Generic;

namespace OpenStory.Framework.Model.Auth
{
    /// <summary>
    /// Provides methods for data operation with characters.
    /// </summary>
    public interface IAuthCharacterProvider
    {
        /// <summary>
        /// Checks if a name is available for use.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns><c>true</c> if the name is available; otherwise, <c>false</c>.</returns>
        bool IsNameAvailable(string name);

        /// <summary>
        /// Saves a new character.
        /// </summary>
        /// <param name="character">The <see cref="AuthCharacter"/> object to save.</param>
        /// <returns>a <see cref="CharacterCreateResult"/> for the opertaion.</returns>
        CharacterCreateResult SaveNew(AuthCharacter character);

        /// <summary>
        /// Removes a character completely.
        /// </summary>
        /// <param name="characterId">The identifier of the character to remove.</param>
        /// <returns></returns>
        CharacterRemoveResult Remove(int characterId);

        /// <summary>
        /// Retrieves an instance of <see cref="AuthCharacter"/>.
        /// </summary>
        /// <param name="characterId">The identifier for the character.</param>
        /// <returns>an instance of <see cref="AuthCharacter"/>, or <c>null</c> if there was no match.</returns>
        AuthCharacter GetAuthCharacter(int characterId);

        /// <summary>
        /// Retrieves all characters for an account.
        /// </summary>
        /// <param name="accountId">The identifier for the account.</param>
        /// <returns>a <see cref="List{AuthCharacter}"/> containing all characters for the account.</returns>
        List<AuthCharacter> GetAuthCharacters(int accountId);

        /// <summary>
        /// Retrieves all characters for an account and a world.
        /// </summary>
        /// <param name="accountId">The identifier for the account.</param>
        /// <param name="worldId">The identifier for the world.</param>
        /// <returns>a <see cref="List{AuthCharacter}"/> containing all characters for the account that reside in the world.</returns>
        List<AuthCharacter> GetAuthCharacters(int accountId, int worldId);
    }
}
