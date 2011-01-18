using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Data
{
    /// <summary>
    /// Controller class for the [World] table.
    /// </summary>
    public static class WorldDataEngine
    {
        /// <summary>
        /// Gets a WorldData object for a world by its ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>A WorldData object representing the record in the database, or null if none was found.</returns>
        public static WorldData GetWorldById(int worldId)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM World WHERE WorldId=@worldId");
            command.Parameters.Add("@worldId", SqlDbType.TinyInt).Value = worldId;
            WorldData world = null;
            DbUtils.InvokeForSingle(command, record => world = GetWorldData(record));
            return world;
        }

        private static WorldData GetWorldData(IDataRecord record)
        {
            return new WorldData(
                (byte) record["WorldId"],
                (string) record["WorldName"],
                (byte) record["ChannelCount"]);
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> over all the worlds in the database.
        /// </summary>
        /// <returns>An IEnumerable{<see cref="WorldData"/>} with all the worlds in the database.</returns>
        public static IEnumerable<WorldData> GetAllWorlds()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM World");
            return DbUtils.GetRecordSetIterator(command).Select(GetWorldData);
        }
    }
}
