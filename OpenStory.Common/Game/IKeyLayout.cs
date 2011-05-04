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
        IKeyBinding GetKeyBinding(byte keyId);

        /// <summary>
        /// Sets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to set the key binding of.</param>
        /// <param name="type">The new action type for the key binding.</param>
        /// <param name="action">The new action for the key binding.</param>
        void SetKeyBinding(byte keyId, byte type, int action);
    }
}