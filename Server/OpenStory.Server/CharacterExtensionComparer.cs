using System.Collections.Generic;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server
{
    /// <summary>
    /// Compares objects that refer to characters.
    /// </summary>
    public class CharacterExtensionComparer : EqualityComparer<ICharacterExtension>
    {
        private readonly IEqualityComparer<CharacterKey> keyComparer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterExtensionComparer"/> class.
        /// </summary>
        /// <param name="keyComparer">The <see cref="IEqualityComparer{CharacterKey}" /> to use internally.</param>
        public CharacterExtensionComparer(IEqualityComparer<CharacterKey> keyComparer)
        {
            this.keyComparer = keyComparer;
        }

        /// <inheritdoc />
        public override bool Equals(ICharacterExtension x, ICharacterExtension y)
        {
            return this.keyComparer.Equals(x.Key, y.Key);
        }

        /// <inheritdoc />
        public override int GetHashCode(ICharacterExtension obj)
        {
            return obj != null ? this.keyComparer.GetHashCode(obj.Key) : 0;
        }
    }
}