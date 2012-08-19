using System.Collections.Generic;

namespace OpenStory.Server.Auth.Data.Providers
{
    /// <summary>
    /// Provides methods for data operation with characters.
    /// </summary>
    public interface ICharacterDataProvider
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
    }
}
