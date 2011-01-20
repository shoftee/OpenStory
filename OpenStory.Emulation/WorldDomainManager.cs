using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Tools;
using OpenStory.Emulation.Helpers;
using OpenStory.Server.Data;

namespace OpenStory.Emulation
{
    [ServerModule(InitializationStage.Worlds)]
    sealed class WorldDomainManager
    {

        private static readonly WorldDomainManager Instance = new WorldDomainManager();
        private WorldDomainManager()
        {
            manager = new AppDomainManager();
            worldAppDomains = new List<AppDomain>();           
        }


        private AppDomainManager manager;
        private List<AppDomain> worldAppDomains;

        [InitializationMethod]
        private static bool Initialize()
        {
            return Instance.LoadWorlds();
        }

        private bool LoadWorlds()
        {
            return WorldDataEngine.GetAllWorlds().All(InitializeWorld);
        }

        private bool InitializeWorld(WorldData world)
        {
            AppDomain newWorldDomain = AppDomainHelpers.GetNewDomain("OpenStory-" + world.WorldName);

            worldAppDomains.Add(newWorldDomain);

            Log.WriteInfo("Loaded world {0}({1}) into new domain '{2}'.", world.WorldName, world.WorldId, newWorldDomain.FriendlyName);
            return true;
        }

    }
}
