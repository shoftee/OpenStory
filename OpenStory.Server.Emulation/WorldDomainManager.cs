using System;
using System.Linq;
using System.Security.Policy;
using OpenStory.Common.Tools;
using OpenStory.Server.Data;

namespace OpenStory.Server.Emulation
{
    [ServerModule(InitializationStage.Worlds)]
    class WorldDomainManager
    {

        private static readonly AppDomainSetup DefaultAppDomainSetup =
            new AppDomainSetup
            {
                ApplicationName = "OpenStory-ServerEmulator",
                LoaderOptimization = LoaderOptimization.SingleDomain
            };

        private static AppDomainManager manager;
        private static AppDomain[] worldAppDomains;

        private const int WorldCount = 1;

        [InitializationMethod]
        private static bool Initialize()
        {
            manager = new AppDomainManager();
            worldAppDomains = new AppDomain[WorldCount];
            return LoadWorlds();
        }

        private static bool LoadWorlds()
        {
            return WorldDataEngine.GetAllWorlds().All(InitializeWorld);
        }

        private static bool InitializeWorld(WorldData world)
        {
            var evidence = new Evidence(AppDomain.CurrentDomain.Evidence);
            AppDomain newWorldDomain = manager.CreateDomain("AppDomain-" + world.WorldName, evidence, DefaultAppDomainSetup);
            newWorldDomain.UnhandledException += UnhandledExceptionHandler;
            newWorldDomain.DomainUnload += DomainUnloadHandler;
            Log.WriteInfo("Loaded world {0}({1}) into new domain '{2}'.", world.WorldName, world.WorldId, newWorldDomain.FriendlyName);
            return true;
        }

        private static void DomainUnloadHandler(object sender, EventArgs e)
        {
            AppDomain domain = sender as AppDomain;
            if (domain == null)
            {
                return;
            }

            Log.WriteInfo("[{0}] Domain unloaded.", domain.FriendlyName);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            AppDomain domain = sender as AppDomain;
            if (domain == null)
            {
                return;
            }

            if (!args.IsTerminating)
            {
                Log.WriteInfo("[{0}] Unhandled exception: {1}", domain.FriendlyName, args.ExceptionObject.ToString());
            }
            else
            {
                Log.WriteError("[{0}] Fatal unhandled exception: {1} ", domain.FriendlyName, args.ExceptionObject.ToString());
                Log.WriteError("The process will now terminate.");
            }
        }

    }
}
