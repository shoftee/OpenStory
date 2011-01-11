using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OpenMaple.Data;
using OpenMaple.IO;
using OpenMaple.Tools;

namespace OpenMaple.Game
{
    class KeyLayout
    {
        public const int KeyCount = 90;

        private List<KeyBinding> bindings;

        public int OwnerId { get; private set; }

        private KeyLayout()
        {
            this.bindings = new List<KeyBinding>(KeyCount);
        }

        private KeyLayout(int ownerId)
            : this()
        {
            this.OwnerId = ownerId;
        }

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
            CharacterEngine.SaveKeyBindings(this.OwnerId, bindings);
        }

        public static KeyLayout LoadFromDb(int ownerId)
        {
            // NOTE: Consider moving this to a more DB-centric class
            var layout = new KeyLayout(ownerId);

            int loaded = CharacterEngine.SelectKeyBindings(ownerId, layout.ReadKeyBinding);
            if (loaded < KeyCount)
            {
                Log.WriteError("Character {0} has only {1} out of {2} key bindings set.", ownerId, loaded, KeyCount);
                return null;
            }
            return layout;
        }

        public static KeyLayout GetDefault(int newOwnerId)
        {
            // TODO: Finish this later.
            throw new NotImplementedException();
        }

        private void ReadKeyBinding(IDataRecord record)
        {
            var keyId = (byte) record["KeyId"];
            var actionTypeId = (byte) record["ActionTypeId"];
            var actionId = (int) record["ActionId"];

            bindings[keyId] = new KeyBinding(actionTypeId, actionId);
        }

        void WriteData(PacketBuilder builder)
        {
            KeyBinding binding;
            for (int i = 0; i < KeyCount; i++)
            {
                binding = this.bindings[i];
                builder.WriteByte(binding.ActionTypeId);
                builder.WriteInt(binding.ActionId);
            }
        }
    }
}
