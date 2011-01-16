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
            using (var query = new SqlCommand(SelectBuddiesQuery))
            {
                query.AddParameter("@characterId", SqlDbType.Int, characterId);
                foreach (IDataRecord record in DbUtils.GetRecordSetIterator(query))
                {
                    recordCallback(record);
                }
            }
        }
    }
}