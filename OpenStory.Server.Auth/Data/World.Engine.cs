using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using OpenStory.Server.Data;

namespace OpenStory.Server.Auth.Data
{
    public sealed partial class World
    {
        /// <summary>
        /// Gets a WorldData object for a world by its ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>A WorldData object representing the record in the database, or null if none was found.</returns>
        public static World GetWorldById(int worldId)
        {
            using (var command = new SqlCommand("SELECT * FROM World WHERE WorldId=@worldId"))
            {
                command.Parameters.Add("@worldId", SqlDbType.TinyInt).Value = worldId;
                World world = null;
                DbHelpers.InvokeForSingle(command, record => world = new World(record));
                return world;
            }
        }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> over all the worlds in the database.
        /// </summary>
        /// <returns>A sequence with all the worlds in the database.</returns>
        public static IEnumerable<World> GetAllWorlds()
        {
            var command = new SqlCommand("SELECT * FROM World ORDER BY WorldId");
            return DbHelpers.Enumerate(command).Select(record => new World(record));
        }
    }
}