using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Server.Game;

namespace OpenStory.Server.Data
{
    public partial class Character
    {
        private const string SelectName =
            "SELECT Name FROM Character WHERE Name = @name";
        private const string SelectCharacterById =
            "SELECT TOP 1 * FROM Character WHERE CharacterId = @characterId";
        private const string SelectBindingsByCharacterId =
            "SELECT KeyId, ActionTypeId, ActionId " +
            "FROM KeyLayoutEntry " +
            "WHERE CharacterId = @characterId";

        /// <summary>
        /// Checks if a name is available for use.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>true if the name is not taken; otherwise, false.</returns>
        public static bool IsNameAvailable(string name)
        {
            using (var query = new SqlCommand(SelectName))
            {
                query.Parameters.Add("@name", SqlDbType.VarChar, 12).Value = name;
                return query.Enumerate().Any();
            }
        }

        /// <summary>
        /// Invokes a callback for a Character record.
        /// </summary>
        /// <param name="characterId">The character ID to query.</param>
        /// <param name="recordCallback">The callback to invoke.</param>
        /// <returns>True if there was a character record found; otherwise, false.</returns>
        public static bool SelectCharacter(int characterId, Action<IDataRecord> recordCallback)
        {
            using (var query = new SqlCommand(SelectCharacterById))
            {
                query.Parameters.Add("@characterId", SqlDbType.Int).Value = characterId;
                return DbHelpers.InvokeForSingle(query, recordCallback);
            }
        }

        /// <summary>
        /// Invokes a callback for the data records of all key bindings belonging to the given character ID.
        /// </summary>
        /// <param name="characterId">The character ID to query.</param>
        /// <param name="recordCallback">The callback to invoke.</param>
        /// <returns>The number of records in the result set.</returns>
        public static int SelectKeyBindings(int characterId, Action<IDataRecord> recordCallback)
        {
            using (var query = new SqlCommand(SelectBindingsByCharacterId))
            {
                query.Parameters.Add("@characterId", SqlDbType.Int).Value = characterId;
                return DbHelpers.InvokeForAll(query, recordCallback);
            }
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
            if (bindings == null)
            {
                throw new ArgumentNullException("bindings");
            }
            if (bindings.Count != GameConstants.KeyCount)
            {
                throw new ArgumentException("There must be exactly " + GameConstants.KeyCount +
                                            " bindings in the given list.");
            }

            byte[] binaryData = BufferNewBindings(bindings);
            if (binaryData.Length == 0)
            {
                return 0;
            }

            SaveKeyBindings(binaryData, characterId);

            // Don't forget to set HasChanged to false.
            foreach (KeyBinding binding in bindings.Where(b => b.HasChanged))
            {
                binding.HasChanged = false;
            }
            return binaryData.Length / 6;
        }

        private static byte[] BufferNewBindings(List<KeyBinding> bindings)
        {
            int length = 0;
            byte[] binaryData;
            using (var buffer = new MemoryStream(GameConstants.KeyCount * 6))
            {
                for (byte i = 0; i < GameConstants.KeyCount; i++)
                {
                    KeyBinding binding = bindings[i];
                    if (!binding.HasChanged)
                    {
                        continue;
                    }
                    length += 6;
                    buffer.WriteByte(i);
                    buffer.WriteByte(binding.ActionTypeId);
                    buffer.Write(BitConverter.GetBytes(binding.ActionId), 0, 4);
                }

                // Copy the binary data to a new array.
                binaryData = new byte[length];
                Buffer.BlockCopy(buffer.GetBuffer(), 0, binaryData, 0, length);
            }
            return binaryData;
        }

        private static void SaveKeyBindings(byte[] binaryData, int characterId)
        {
            int length = binaryData.Length;
            using (var command = new SqlCommand("up_SaveKeyBindings"))
            {
                command.Parameters.Add("@CharacterId", SqlDbType.Int).Value = characterId;
                command.Parameters.Add("@BinaryData", SqlDbType.VarBinary, length).Value = binaryData;
                DbHelpers.ExecuteStoredProcedure(command);
            }
        }
    }
}