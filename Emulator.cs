using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenMaple.Tools;

namespace OpenMaple
{
    [ServerModule(InitializationStage.StartUp)]
    public sealed class Emulator
    {
        private IEnumerable<MetadataPair<Type, ServerModuleAttribute>> serverModulesInternal;
        private IEnumerable<MetadataPair<Type, ServerModuleAttribute>> ServerModules
        {
            get
            {
                return serverModulesInternal ?? (serverModulesInternal = GetServerModules());
            }
        }

        public bool IsRunning { get; private set; }

        public Emulator()
        {
            if (!this.Initialize())
            {
                Log.WriteError("Server startup failed.");
            }
            else
            {
                this.IsRunning = true;
            }
        }

        private bool Initialize()
        {
            var initializationList = ServerModules.
                GroupBy(pair => pair.Attribute.InitializationStage, pair => pair.MemberInfo).
                OrderBy(group => group.Key);

            foreach (var group in initializationList)
            {
                Log.WriteInfo("Initialization stage: {0}", Enum.GetName(typeof(InitializationStage), group.Key));

                var query = group.SelectMany(GetInitializationMethodsByType).AsParallel();
                 
                if (!query.All(ReflectionUtils.InvokeFunc<bool>))
                {
                    Log.WriteError("Initialization failed, an initialization method returned 'false'.");
                    return false;
                }
            }
            return true;
        }

        [InitializationMethod]
        private static bool TestInit1()
        {
            Log.WriteInfo("Initialization test method #1.");
            return true;
        }

        [InitializationMethod]
        private static bool TestInit2()
        {
            Log.WriteInfo("Initialization test method #2.");
            return false;
        }

        private static IEnumerable<MetadataPair<Type, ServerModuleAttribute>> GetServerModules()
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    where !assembly.GlobalAssemblyCache
                    from type in assembly.GetTypes()
                    let moduleAttribute = ReflectionUtils.GetAttribute<ServerModuleAttribute>(type)
                    where moduleAttribute != null
                    select new MetadataPair<Type, ServerModuleAttribute>(type, moduleAttribute));
        }

        private static IEnumerable<MethodInfo> GetInitializationMethodsByType(Type type)
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic).
                Where(ReflectionUtils.HasAttribute<InitializationMethodAttribute>);
        }
    }
}
