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

        public int PlayerId { get; private set; }

        public KeyBinding GetKeyBinding(byte keyId)
        {
            return this.bindings[keyId];
        }

        public void SetKeyBinding(byte keyId, byte type, int action)
        {
            this.bindings[keyId].Change(type, action);
        }

        public void SaveToDb()
        {
            CharacterEngine.SaveKeyBindings(this.PlayerId, this.bindings);
        }

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