namespace OpenStory.Common.Game
{
    /// <summary>
    /// Represents a key mapping for an in-game action.
    /// </summary>
    public sealed class KeyBinding
    {
        /// <summary>
        /// Gets the type of the action type identifier for the <see cref="KeyBinding"/>.
        /// </summary>
        public byte ActionTypeId { get; private set; }

        /// <summary>
        /// Gets the action identifier for the <see cref="KeyBinding"/>.
        /// </summary>
        public int ActionId { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="KeyBinding"/>.
        /// </summary>
        /// <param name="actionTypeId">The action type identifier for the binding.</param>
        /// <param name="actionId">The action identifier for the binding.</param>
        public KeyBinding(byte actionTypeId, int actionId)
        {
            this.ActionTypeId = actionTypeId;
            this.ActionId = actionId;
        }
    }
}
