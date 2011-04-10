namespace OpenStory.Common.Game
{
    /// <summary>
    /// Provides methods for accessing and modifying a key-mapped game action.
    /// </summary>
    public interface IKeyBinding
    {
        /// <summary>
        /// Gets the type of the action for the KeyBinding.
        /// </summary>
        byte ActionTypeId { get; }

        /// <summary>
        /// Gets the action ID for the KeyBinding.
        /// </summary>
        int ActionId { get; }

        /// <summary>
        /// Assigns new pair of action type and action ID to the KeyBinding.
        /// </summary>
        /// <param name="type">The new action type ID.</param>
        /// <param name="action">The new action ID.</param>
        void Change(byte type, int action);
    }
}