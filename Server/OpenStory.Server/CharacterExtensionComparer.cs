using System.Collections.Generic;

namespace OpenStory.Server
{
    /// <summary>
    /// Compares objects that refer to characters.
    /// </summary>
    public class CharacterExtensionComparer : EqualityComparer<ICharacterExtension>
    {
        private static readonly EqualityComparer<CharacterKey> DefaultKeyComparer = EqualityComparer<CharacterKey>.Default;

        private static readonly CharacterExtensionComparer InternalInstance = new CharacterExtensionComparer();
        private CharacterExtensionComparer() { }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static CharacterExtensionComparer Instance { get { return InternalInstance; } }

        /// <inheritdoc />
        public override bool Equals(ICharacterExtension x, ICharacterExtension y)
        {
            return DefaultKeyComparer.Equals(x.Key, y.Key);
        }

        /// <inheritdoc />
        public override int GetHashCode(ICharacterExtension obj)
        {
            return obj != null ? DefaultKeyComparer.GetHashCode(obj.Key) : 0;
        }
    }
}