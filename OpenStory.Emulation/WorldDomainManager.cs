using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Tools;
using OpenStory.Emulation.Helpers;
using OpenStory.Server.Data;

namespace OpenStory.Emulation
{
    sealed class WorldDomainManager
    {
        private Dictionary<byte, AppDomain> domains;
        private Dictionary<byte, WorldData> data;

        private bool isInitialized;

        public WorldDomainManager()
        {
            this.isInitialized = false;
        }

        public bool Initialize()
        {
            if (this.isInitialized)
            {
                throw new InvalidOperationException("The WorldDomainManager is already initialized.");
            }
            this.data = WorldDataEngine.GetAllWorlds().OrderBy(w => w.WorldId).ToDictionary(w => w.WorldId, w => w);
            this.domains = new Dictionary<byte, AppDomain>(this.data.Count);
            return this.data.Values.All(InitializeWorld);
        }

        private bool InitializeWorld(WorldData world)
        {
            AppDomain newWorldDomain = AppDomainHelpers.GetNewDomain("OpenStory-" + world.WorldName);

            this.domains.Add(world.WorldId, newWorldDomain);

            Log.WriteInfo("Loaded world {0}({1}) into new domain '{2}'.", world.WorldName, world.WorldId, newWorldDomain.FriendlyName);
            return true;
        }

        /// <summary>
        /// Gets the <see cref="AppDomain"/> for a world by the world's ID.
        /// </summary>
        /// <param name="worldId">The world ID to query.</param>
        /// <exception cref="KeyNotFoundException">
        /// The exception is thrown when no world with the given ID is found.
        /// </exception>
        /// <returns>The AppDomain of the world.</returns>
        public AppDomain GetDomainByWorldId(byte worldId)
        {
            this.CheckInitialized();
            return this.domains[worldId];
        }

        /// <summary>
        /// Gets the <see cref="WorldData"/> for a world by the world's ID.
        /// </summary>
        /// <param name="worldId">The world ID to query.</param>
        /// <exception cref="KeyNotFoundException">
        /// The exception is thrown when no world with the given ID is found.
        /// </exception>
        /// <returns>The WorldData object for the world.</returns>
        public WorldData GetDataByWorldId(byte worldId)
        {
            this.CheckInitialized();
            return this.data[worldId];
        }

        private void CheckInitialized()
        {
            if (!this.isInitialized)
            {
                throw new InvalidOperationException("The WorldDomainManager has not been initialized.");
            }
        }
    }
}
