using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Tools;
using OpenStory.Server.Data;
using OpenStory.Server.Emulation.Helpers;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// A manager class for game worlds.
    /// </summary>
    internal sealed class UniverseManager
    {
        private List<World> data;
        private Dictionary<int, AppDomain> domains;

        private bool isInitialized;

        public UniverseManager()
        {
            this.isInitialized = false;
        }

        public bool Initialize()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("The UniverseManager is already initialized.");
            }
            this.data = World.GetAllWorlds().ToList();
            this.domains = new Dictionary<int, AppDomain>(this.data.Count);
            return this.data.All(this.InitializeWorld);
        }

        private bool InitializeWorld(World world)
        {
            AppDomain newWorldDomain = AppDomainHelpers.GetNewDomain("OpenStory-" + world.WorldName);

            this.domains.Add(world.WorldId, newWorldDomain);

            Log.WriteInfo("Loaded world {0}({1}) into new domain '{2}'.", world.WorldName, world.WorldId,
                          newWorldDomain.FriendlyName);
            return true;
        }

        /// <summary>
        /// Gets the <see cref="AppDomain"/> for a world by the world's ID.
        /// </summary>
        /// <param name="worldId">The world ID to query.</param>
        /// <returns>The AppDomain of the world.</returns>
        public AppDomain GetDomainByWorldId(byte worldId)
        {
            this.CheckInitialized();
            return this.domains[worldId];
        }

        /// <summary>
        /// Gets the <see cref="OpenStory.Server.Data.World"/> for a world by the world's ID.
        /// </summary>
        /// <param name="worldId">The world ID to query.</param>
        /// <returns>The WorldData object for the world.</returns>
        public World GetDataByWorldId(byte worldId)
        {
            this.CheckInitialized();
            return this.data[worldId];
        }

        private void CheckInitialized()
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("The UniverseManager has not been initialized.");
            }
        }
    }
}