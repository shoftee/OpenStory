using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace OpenStory.Server.Data
{
    public partial class Character
    {
        private const string SelectName =
            "SELECT Name FROM Character WHERE Name = @name";
        private const string SelectCharacterById =
            "SELECT TOP 1 * FROM Character WHERE CharacterId = @characterId";

        /// <summary>
        /// Checks if a name is available for use.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns><c>true</c> if the name is not taken; otherwise, <c>false</c>.</returns>
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
        /// <returns><c>true</c> if there was a character record found; otherwise, <c>false</c>.</returns>
        public static bool SelectCharacter(int characterId, Action<IDataRecord> recordCallback)
        {
            using (var query = new SqlCommand(SelectCharacterById))
            {
                query.Parameters.Add("@characterId", SqlDbType.Int).Value = characterId;
                return DbHelpers.InvokeForSingle(query, recordCallback);
            }
        }
    }
}