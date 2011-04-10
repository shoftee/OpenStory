using System;
using OpenStory.Common.Game;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a key mapping for an in-game action.
    /// </summary>
    public class KeyBinding : IKeyBinding
    {
        /// <summary>
        /// Initializes a new instance of KeyBinding with an action type ID and an action ID.
        /// </summary>
        /// <param name="actionTypeId">The action type ID for the KeyBinding.</param>
        /// <param name="actionId">The action ID for the KeyBinding.</param>
        public KeyBinding(byte actionTypeId, int actionId)
        {
            this.ActionTypeId = actionTypeId;
            this.ActionId = actionId;
        }

        /// <summary>
        /// Gets or sets whether the key binding information has changed since it was initialized.
        /// </summary>
        public bool HasChanged { get; set; }

        /// <summary>
        /// Gets the type of the action for the KeyBinding.
        /// </summary>
        public byte ActionTypeId { get; private set; }

        /// <summary>
        /// Gets the action ID for the KeyBinding.
        /// </summary>
        public int ActionId { get; private set; }

        /// <summary>
        /// Assigns new pair of action type and action ID to the KeyBinding.
        /// </summary>
        /// <param name="type">The new action type ID.</param>
        /// <param name="action">The new action ID.</param>
        public void Change(byte type, int action)
        {
            if (this.ActionTypeId == type && this.ActionId == action) return;

            this.HasChanged = true;
            this.ActionTypeId = type;
            this.ActionId = action;
        }
    }
}