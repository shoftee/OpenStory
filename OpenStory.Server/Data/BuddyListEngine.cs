using System;
using System.Data;
using System.Data.SqlClient;

namespace OpenStory.Server.Data
{
    internal static class BuddyListEngine
    {
        private const string SelectBuddiesQuery =
            "SELECT * " +
            "FROM BuddyListEntry " +
            "WHERE [CharacterId]=@characterId";

        public static void LoadByCharacterId(int characterId, Action<IDataRecord> recordCallback)
        {
            SqlCommand query = new SqlCommand(SelectBuddiesQuery);
            query.Parameters.Add("@characterId", SqlDbType.Int).Value = characterId;
            DbUtils.InvokeForAll(query, recordCallback);
        }
    }
}