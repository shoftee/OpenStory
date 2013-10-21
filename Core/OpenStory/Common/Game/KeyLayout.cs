using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// Represents a collection of key mappings for in-game actions.
    /// </summary>
    public sealed class KeyLayout
    {
        private readonly List<KeyBinding> bindings;

        /// <summary>
        /// Gets a read-only list of the key bindings.
        /// </summary>
        public ReadOnlyCollection<KeyBinding> Bindings
        {
            get { return this.bindings.ToReadOnly(); }
        }

        private KeyLayout()
        {
            this.bindings = new List<KeyBinding>(GameConstants.KeyCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyLayout"/> class.
        /// </summary>
        /// <param name="bindings">The bindings to use for this instance.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="bindings"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="bindings"/> does not have exactly <see cref="GameConstants.KeyCount"/> elements.
        /// </exception>
        public KeyLayout(ICollection<KeyBinding> bindings)
            : this()
        {
            Guard.NotNull(() => bindings, bindings);

            if (bindings.Count != GameConstants.KeyCount)
            {
                var message = string.Format(CommonStrings.WrongKeyBindingCount, GameConstants.KeyCount);
                throw new ArgumentException(message);
            }

            this.bindings.AddRange(bindings);
        }

        /// <summary>
        /// Gets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to query the key binding of.</param>
        /// <returns>a <see cref="KeyBinding"/> object representing the binding for the key.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="keyId"/> is greater than <see cref="GameConstants.KeyCount"/>.
        /// </exception>
        public KeyBinding GetKeyBinding(byte keyId)
        {
            ThrowIfInvalidId(keyId);

            return this.bindings[keyId];
        }

        /// <summary>
        /// Sets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to set the key binding of.</param>
        /// <param name="type">The new action type for the key binding.</param>
        /// <param name="action">The new action for the key binding.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="keyId"/> is greater than <see cref="GameConstants.KeyCount"/>.
        /// </exception>
        public void SetKeyBinding(byte keyId, byte type, int action)
        {
            ThrowIfInvalidId(keyId);

            this.bindings[keyId] = new KeyBinding(type, action);
        }

        private static void ThrowIfInvalidId(byte keyId)
        {
            if (GameConstants.KeyCount <= keyId)
            {
                throw new ArgumentOutOfRangeException("keyId", keyId, CommonStrings.InvalidKeyIdentifier);
            }
        }
    }
}
