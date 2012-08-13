using System;
using System.Collections.Generic;
using System.Data;
using OpenStory.Common.Game;
using OpenStory.Common.Tools;
using OpenStory.Server.Data;

namespace OpenStory.Server.Game
{
    /// <summary>
    /// Represents a collection of key mappings for in-game actions.
    /// </summary>
    public class KeyLayout : IKeyLayout
    {
        /// <summary>
        /// The number of key bindings.
        /// </summary>
        private List<KeyBinding> bindings;

        private KeyLayout()
        {
            this.bindings = new List<KeyBinding>(GameConstants.KeyCount);
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

        #region IKeyLayout Members

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="keyId"/> is negative or greater than <see cref="GameConstants.KeyCount"/>.
        /// </exception>
        public IKeyBinding GetKeyBinding(byte keyId)
        {
            ThrowIfInvalidId(keyId);
            return this.bindings[keyId];
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="keyId"/> is negative or greater than <see cref="GameConstants.KeyCount"/>.
        /// </exception>
        public void SetKeyBinding(byte keyId, byte type, int action)
        {
            ThrowIfInvalidId(keyId);
            this.bindings[keyId].Change(type, action);
        }

        #endregion

        /// <summary>
        /// Saves the key bindings.
        /// </summary>
        public void SaveToDb()
        {
            Character.SaveKeyBindings(this.PlayerId, this.bindings);
        }

        /// <summary>
        /// Loads the key bindings for a player.
        /// </summary>
        /// <param name="playerId">The ID of the player whose key bindings to load.</param>
        /// <returns>a <see cref="KeyLayout"/> object for the given player.</returns>
        public static KeyLayout LoadFromDb(int playerId)
        {
            // NOTE: Consider moving this to a more DB-centric class
            var layout = new KeyLayout(playerId);

            int loaded = Character.SelectKeyBindings(playerId, layout.ReadKeyBinding);
            if (loaded < GameConstants.KeyCount)
            {
                Log.WriteError("Character {0} has only {1} out of {2} key bindings set.", playerId, loaded,
                               GameConstants.KeyCount);
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

        private static void ThrowIfInvalidId(byte keyId)
        {
            if (keyId < 0 || GameConstants.KeyCount <= keyId)
            {
                throw new ArgumentOutOfRangeException(
                    "keyId", keyId, "'keyId' must be a valid key identifier.");
            }
        }
    }
}