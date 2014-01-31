using System.Collections.Generic;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;

namespace OpenStory.Server.World
{
    /// <summary>
    /// Provides... world info...
    /// </summary>
    public class WorldInfoProvider : IWorldInfoProvider
    {
        /// <inheritdoc/>
        public WorldInfo GetWorldById(int id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public IEnumerable<WorldInfo> GetAllWorlds()
        {
            throw new System.NotImplementedException();
        }
    }
}