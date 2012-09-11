using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Auth.Data.Providers
{
    /// <summary>
    /// Provides methods for accessing world configuration data.
    /// </summary>
    public interface IWorldInfoProvider
    {
        /// <summary>
        /// Gets the configuration data for a world.
        /// </summary>
        /// <param name="id">The identifier of the world.</param>
        /// <returns>an instance of <see cref="WorldInfo"/> representing the record in the database, or <c>null</c> if none was found.</returns>
        WorldInfo GetWorldById(int id);

        /// <summary>
        /// Gets an <see cref="IEnumerable{World}"/> over all the worlds in the database.
        /// </summary>
        /// <returns>a sequence with all the worlds in the database.</returns>
        IEnumerable<WorldInfo> GetAllWorlds();
    }
}
