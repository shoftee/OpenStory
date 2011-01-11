using System;
using System.Data;
using System.Data.SqlClient;

namespace OpenMaple.Data
{
    static class BuddyListEngine
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
                foreach (var record in DbUtils.GetRecordSetIterator(query))
                {
                    recordCallback(record);
                }
            }
        }
    }
}
