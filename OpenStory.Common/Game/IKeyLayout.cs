using System;

namespace OpenStory.Common.Game
{
    /// <summary>
    /// Provides methods for manipulating key bindings.
    /// </summary>
    public interface IKeyLayout
    {
        /// <summary>
        /// Gets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to query the key binding of.</param>
        /// <returns>A KeyBinding object representing the binding for the key.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <c>keyId</c> is negative or greater than <see cref="Constants.KeyCount"/>.
        /// </exception>
        IKeyBinding GetKeyBinding(byte keyId);

        /// <summary>
        /// Sets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to set the key binding of.</param>
        /// <param name="type">The new action type for the key binding.</param>
        /// <param name="action">The new action for the key binding.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <c>keyId</c> is negative or greater than <see cref="Constants.KeyCount"/>.
        /// </exception>
        void SetKeyBinding(byte keyId, byte type, int action);
    }
}