namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Provides a reference to a character.
    /// </summary>
    public interface ICharacterExtension
    {
        /// <summary>
        /// Gets the key of the associated character.
        /// </summary>
        CharacterKey Key { get; }
    }
}