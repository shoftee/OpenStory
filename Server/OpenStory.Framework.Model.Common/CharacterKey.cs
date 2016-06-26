using System;

namespace OpenStory.Framework.Model.Common
{
    /// <summary>
    /// Represents the identifiers for a character.
    /// </summary>
    [Serializable]
    public sealed class CharacterKey : IEquatable<CharacterKey>
    {
        /// <summary>
        /// Gets the unique numeric identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the unique string identifier.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterKey"/> type.
        /// </summary>
        /// <param name="id">The numeric identifier.</param>
        /// <param name="name">The string identifier.</param>
        public CharacterKey(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        /// <inheritdoc />
        public bool Equals(CharacterKey other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (Equals(other, null))
            {
                return false;
            }

            return this.Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (Equals(obj, null))
            {
                return false;
            }

            var key = obj as CharacterKey;
            return key != null && this.Id == key.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.Id;
        }

        /// <summary>
        /// Checks the provided objects for equality.
        /// </summary>
        public static bool operator ==(CharacterKey left, CharacterKey right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Checks the provided objects for inequality.
        /// </summary>
        public static bool operator !=(CharacterKey left, CharacterKey right)
        {
            return !Equals(left, right);
        }
    }
}
