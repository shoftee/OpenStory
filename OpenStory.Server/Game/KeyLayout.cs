using System;
using System.Collections.Generic;
using System.Data;
using OpenStory.Common.Tools;
using OpenStory.Server.Data;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a key mapping for game actions.
    /// </summary>
    public class KeyLayout
    {
        /// <summary>
        /// The number of key bindings.
        /// </summary>
        public const int KeyCount = 90;
        private List<KeyBinding> bindings;

        private KeyLayout()
        {
            this.bindings = new List<KeyBinding>(KeyCount);
        }

        private KeyLayout(int playerId)
            : this()
        {
            this.PlayerId = playerId;
        }

        /// <summary>
        /// The ID of the player this KeyLayout belongs to.
        /// </summary>
        public int PlayerId { get; private set; }

        /// <summary>
        /// Gets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to query the key binding of.</param>
        /// <returns>A KeyBinding object representing the binding for the key.</returns>
        public KeyBinding GetKeyBinding(byte keyId)
        {
            if (keyId < 0 || KeyCount < keyId)
            {
                throw new ArgumentOutOfRangeException("keyId", keyId, "'keyId' must be between 0 and " + KeyCount + " inclusive.");
            }
            return this.bindings[keyId];
        }

        /// <summary>
        /// Sets the key binding for a key.
        /// </summary>
        /// <param name="keyId">The key to set the key binding of.</param>
        /// <param name="type">The new action type for the key binding.</param>
        /// <param name="action">The new action for the key binding.</param>
        public void SetKeyBinding(byte keyId, byte type, int action)
        {
            if (keyId < 0 || KeyCount < keyId)
            {
                throw new ArgumentOutOfRangeException("keyId", keyId, "'keyId' must be between 0 and " + KeyCount + " inclusive.");
            }
            this.bindings[keyId].Change(type, action);
        }

        /// <summary>
        /// Saves the key bindings.
        /// </summary>
        public void SaveToDb()
        {
            CharacterEngine.SaveKeyBindings(this.PlayerId, this.bindings);
        }

        /// <summary>
        /// Loads the key bindings for a player.
        /// </summary>
        /// <param name="playerId">The ID of the player whose key bindings to load.</param>
        /// <returns>A <see cref="KeyLayout"/> object for the given player.</returns>
        public static KeyLayout LoadFromDb(int playerId)
        {
            // NOTE: Consider moving this to a more DB-centric class
            var layout = new KeyLayout(playerId);

            int loaded = CharacterEngine.SelectKeyBindings(playerId, layout.ReadKeyBinding);
            if (loaded < KeyCount)
            {
                Log.WriteError("Character {0} has only {1} out of {2} key bindings set.", playerId, loaded, KeyCount);
                return null;
            }
            return layout;
        }

        /// <summary>
        /// Gets the default key binding layout initialized for the given player ID.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public static KeyLayout GetDefault(int playerId)
        {
            // TODO: Finish this later.
            throw new NotImplementedException();
        }

        private void ReadKeyBinding(IDataRecord record)
        {
            var keyId = (byte) record["KeyId"];
            var actionTypeId = (byte) record["ActionTypeId"];
            var actionId = (int) record["ActionId"];

            this.bindings[keyId] = new KeyBinding(actionTypeId, actionId);
        }
    }
}