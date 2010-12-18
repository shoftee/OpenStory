using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenMaple.Client
{
    class KeyLayout : IPacketData
    {
        private const int KeyCount = 90;

        private bool hasChanged;
        private KeyBinding[] bindings;

        public int OwnerId { get; private set; }

        private KeyLayout()
        {
            this.bindings = new KeyBinding[KeyCount];
        }

        private KeyLayout(int ownerId) : this()
        {
            this.OwnerId = ownerId;
            this.hasChanged = false;
        }

        public KeyBinding GetKeyBinding(int keyId)
        {
            return this.bindings[keyId];
        }

        public void SetKeyBinding(int keyId, byte type, int action)
        {
            KeyBinding newBinding = new KeyBinding(type, action);
            SetKeyBinding(keyId, newBinding);
        }

        public void SetKeyBinding(int keyId, KeyBinding keyBinding)
        {
            if (!keyBinding.Equals(this.bindings[keyId]))
            {
                this.bindings[keyId] = keyBinding;
                this.hasChanged = true;
            }
        }

        public void SaveToDb()
        {

        }

        public static KeyLayout LoadFromDb(int ownerId)
        {
            // NOTE: Consider moving this to a more DB-centric class
            KeyLayout layout = new KeyLayout(ownerId);
            const string Query = "SELECT [KeyId],[ActionTypeId],[ActionId] FROM [KeyLayoutEntry] WHERE [CharacterId]=@ownerId";
            SqlCommand command = new SqlCommand(Query);
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.OpenMapleConnectionString))
            {
                command.Connection = connection;
                connection.Open();
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    int loaded = 0;
                    while (reader.Read())
                    {
                        KeyBinding binding = new KeyBinding((byte) reader["ActionTypeId"], (int) reader["ActionId"]);
                        layout.bindings[(int) reader["KeyId"]] = binding;
                        loaded++;
                    }
                    if (loaded < KeyCount)
                    {
                        // TODO: Error logging :<
                    }
                }
            }
            return layout;
        }

        public static KeyLayout GetDefault(int newOwnerId)
        {
            // TODO: Finish this later.
            throw new NotImplementedException();
        }

        void IPacketData.WriteData(PacketWriter writer)
        {
            KeyBinding binding;
            for (int i = 0; i < KeyCount; i++)
            {
                binding = this.bindings[i];
                writer.WriteByte(binding.Type);
                writer.WriteInt32(binding.Action);
            }
        }
    }

    struct KeyBinding : IEquatable<KeyBinding>
    {
        public byte Type { get; private set; }
        public int Action { get; private set; }

        public KeyBinding(byte type, int action)
            : this()
        {
            this.Type = type;
            this.Action = action;
        }

        public bool Equals(KeyBinding other)
        {
            return this.Type == other.Type && this.Action == other.Action;
        }
    }
}
