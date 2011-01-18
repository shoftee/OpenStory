using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenStory.Common.Tools;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// The entry-point class for the server.
    /// </summary>
    public sealed class Emulator
    {
        private IEnumerable<MetadataPair<Type, ServerModuleAttribute>> serverModulesInternal;

        /// <summary>
        /// Initializes the Emulator.
        /// </summary>
        public Emulator()
        {
            if (!this.Initialize())
            {
                Log.WriteError("Server startup failed.");
            }
            else
            {
                Log.WriteInfo("Startup successful.");
                this.IsRunning = true;
            }
        }

        private IEnumerable<MetadataPair<Type, ServerModuleAttribute>> ServerModules
        {
            get { return this.serverModulesInternal ?? (this.serverModulesInternal = GetServerModules()); }
        }

        /// <summary>
        /// Denotes whether the emulator is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

        private bool Initialize()
        {
            IOrderedEnumerable<IGrouping<InitializationStage, Type>> initializationList = this.ServerModules.
                GroupBy(pair => pair.Attribute.InitializationStage, pair => pair.MemberInfo).
                OrderBy(group => group.Key);

            foreach (var group in initializationList)
            {
                Log.WriteInfo("Initialization stage: {0}", Enum.GetName(typeof (InitializationStage), group.Key));

                ParallelQuery<MethodInfo> query = group.SelectMany(GetInitializationMethodsByType).AsParallel();

                if (query.All(ReflectionHelpers.InvokeFunc<bool>)) continue;

                Log.WriteError("Initialization failed, an initialization method returned 'false'.");
                return false;
            }
            return true;
        }

        private static IEnumerable<MetadataPair<Type, ServerModuleAttribute>> GetServerModules()
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    where !assembly.GlobalAssemblyCache
                    from type in assembly.GetTypes()
                    let moduleAttribute = ReflectionHelpers.GetAttribute<ServerModuleAttribute>(type)
                    where moduleAttribute != null
                    select new MetadataPair<Type, ServerModuleAttribute>(type, moduleAttribute));
        }

        private static IEnumerable<MethodInfo> GetInitializationMethodsByType(Type type)
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic).
                Where(ReflectionHelpers.HasAttribute<InitializationMethodAttribute>);
        }
    }
}