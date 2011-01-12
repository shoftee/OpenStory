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
        private const string SelectNameCount = "SELECT COUNT(*) FROM [dbo].[Character] WHERE [Name]=@name";

        private const string SelectCharacterById =
            "SELECT TOP 1 * FROM [dbo].[Character] WHERE [CharacterId]=@characterId";

        private const string SelectBindingsByCharacterId =
            "SELECT [KeyId],[ActionTypeId],[ActionId] " +
            "FROM [KeyLayoutEntry] " +
            "WHERE [CharacterId]=@characterId";

        public static bool IsNameAvailable(string name)
        {
            using (var query = new SqlCommand(SelectNameCount))
            {
                DbUtils.AddParameter(query, "@name", SqlDbType.VarChar, 12, name);
                return (DbUtils.GetScalar<int>(query) == 0);
            }
        }

        public static bool SelectCharacter(int characterId, Action<IDataRecord> recordCallback)
        {
            using (var query = new SqlCommand(SelectCharacterById))
            {
                DbUtils.AddParameter(query, "@characterId", SqlDbType.Int, characterId);
                return DbUtils.InvokeForSingle(query, recordCallback);
            }
        }

        public static int SelectKeyBindings(int characterId, Action<IDataRecord> recordCallback)
        {
            using (var query = new SqlCommand(SelectBindingsByCharacterId))
            {
                DbUtils.AddParameter(query, "@characterId", SqlDbType.Int, characterId);
                return DbUtils.InvokeForAll(query, recordCallback);
            }
        }

        /// <summary>
        /// Determines which bindings have changed and updates the database accordingly.
        /// </summary>
        /// <param name="characterId">The character whose bindings to save.</param>
        /// <param name="bindings">The list of bindings for the character.</param>
        public static void SaveKeyBindings(int characterId, List<KeyBinding> bindings)
        {
            if (bindings == null) throw new ArgumentNullException("bindings");
            if (bindings.Count != KeyLayout.KeyCount)
            {
                throw new ArgumentException("There must be exactly " + KeyLayout.KeyCount +
                                            " bindings in the given list.");
            }
            int count = 0;

            // Write the new bindings to a buffer.
            var buffer = new MemoryStream(KeyLayout.KeyCount * 6);
            for (byte i = 0; i < KeyLayout.KeyCount; i++)
            {
                KeyBinding binding = bindings[i];
                if (!binding.HasChanged) continue;
                count++;
                buffer.WriteByte(i);
                buffer.WriteByte(binding.ActionTypeId);
                buffer.Write(BitConverter.GetBytes(binding.ActionId), 0, 4);
            }

            if (count == 0) return;

            // Copy the binary data to a new array.
            int length = count * 6;
            var binaryData = new byte[length];
            Buffer.BlockCopy(buffer.GetBuffer(), 0, binaryData, 0, length);

            buffer.Dispose();

            // Update with the stored proc <3
            using (var command = new SqlCommand("up_SaveKeyBindings"))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 60;

                DbUtils.AddParameter(command, "@CharacterId", SqlDbType.Int, characterId);
                DbUtils.AddParameter(command, "@BinaryData", SqlDbType.VarBinary, length, binaryData);

                DbUtils.ExecuteNonQuery(command);
            }

            // Don't forget to set HasChanged to false.
            foreach (KeyBinding binding in bindings.Where(b => b.HasChanged))
            {
                binding.HasChanged = false;
            }
        }
    }
}