using OpenStory.Framework.Model.Common;

namespace OpenStory.Server
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