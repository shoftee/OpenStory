using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using OpenStory.Server.Game;

namespace OpenStory.Server.Data
{
    static class CharacterEngine
    {
        private const string SelectName =
            "SELECT Name FROM Character WHERE Name=@name";

        private const string SelectCharacterById =
            "SELECT TOP 1 * FROM Character WHERE CharacterId=@characterId";

        private const string SelectBindingsByCharacterId =
            "SELECT KeyId,ActionTypeId,ActionId " +
            "FROM KeyLayoutEntry " +
            "WHERE CharacterId=@characterId";

        public static bool IsNameAvailable(string name)
        {
            SqlCommand query = new SqlCommand(SelectName);
            query.Parameters.Add("@name", SqlDbType.VarChar, 12).Value = name;
            return DbUtils.GetRecordSetIterator(query).Any();
        }

        public static bool SelectCharacter(int characterId, Action<IDataRecord> recordCallback)
        {
            SqlCommand query = new SqlCommand(SelectCharacterById);
            query.Parameters.Add("@characterId", SqlDbType.Int).Value = characterId;
            return DbUtils.InvokeForSingle(query, recordCallback);
        }

        public static int SelectKeyBindings(int characterId, Action<IDataRecord> recordCallback)
        {
            SqlCommand query = new SqlCommand(SelectBindingsByCharacterId);
            query.Parameters.Add("@characterId", SqlDbType.Int).Value = characterId;
            return DbUtils.InvokeForAll(query, recordCallback);
        }

        /// <summary>
        /// Determines which bindings have changed and updates the database accordingly.
        /// </summary>
        /// <param name="characterId">The character whose bindings to save.</param>
        /// <param name="bindings">The list of bindings for the character.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="bindings"/> has an invalid number of elements.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="bindings"/> is <c>null</c>.</exception>
        /// <returns>The number of bindings that were saved.</returns>
        public static int SaveKeyBindings(int characterId, List<KeyBinding> bindings)
        {
            if (bindings == null) throw new ArgumentNullException("bindings");
            if (bindings.Count != KeyLayout.KeyCount)
            {
                throw new ArgumentException("There must be exactly " + KeyLayout.KeyCount +
                                            " bindings in the given list.");
            }
            int count = 0;

            // Write the new bindings to a buffer.
            int length;
            byte[] binaryData;
            using (var buffer = new MemoryStream(KeyLayout.KeyCount * 6))
            {
                for (byte i = 0; i < KeyLayout.KeyCount; i++)
                {
                    KeyBinding binding = bindings[i];
                    if (!binding.HasChanged) continue;
                    count++;
                    buffer.WriteByte(i);
                    buffer.WriteByte(binding.ActionTypeId);
                    buffer.Write(BitConverter.GetBytes(binding.ActionId), 0, 4);
                }

                if (count == 0) return 0;

                // Copy the binary data to a new array.
                length = count * 6;
                binaryData = new byte[length];
                Buffer.BlockCopy(buffer.GetBuffer(), 0, binaryData, 0, length);
            }

            // Update with the stored proc <3
            SqlCommand command = new SqlCommand("up_SaveKeyBindings");
            command.Parameters.Add("@CharacterId", SqlDbType.Int).Value = characterId;
            command.Parameters.Add("@BinaryData", SqlDbType.VarBinary, length).Value = binaryData;
            DbUtils.ExecuteStoredProcedure(command);

            // Don't forget to set HasChanged to false.);
            foreach (KeyBinding binding in bindings.Where(b => b.HasChanged))
            {
                binding.HasChanged = false;
            }
            return count;
        }
    }
}