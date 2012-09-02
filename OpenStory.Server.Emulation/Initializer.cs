using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenStory.Common.Tools;
using OpenStory.Server.Emulation.Helpers;
using OpenStory.Server.Fluent;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// Provides a method to initialize types marked with a <see cref="ServerModuleAttribute"/>
    /// </summary>
    internal static class Initializer
    {
        private static List<MetadataPair<Type, ServerModuleAttribute>> serverModulesInternal;

        /// <summary>
        /// Initializes the classes marked with a <see cref="ServerModuleAttribute"/>.
        /// </summary>
        /// <remarks>
        /// This method calls methods marked with a <see cref="InitializationMethodAttribute"/>
        /// in types marked with a <see cref="ServerModuleAttribute"/>. 
        /// The types will be inspected and initialized in the order given by the 
        /// <see cref="InitializationStage"/> parameter in their ServerModuleAttribute. 
        /// The order in which methods in the same InitializationStage are called is undefined.
        /// </remarks>
        /// <returns>true if initialization was successful; otherwise, false.</returns>
        public static bool Run()
        {
            if (serverModulesInternal == null)
            {
                serverModulesInternal = GetServerModules().ToList();
            }

            var initializationList = serverModulesInternal
                .GroupBy(pair => pair.Attribute.InitializationStage, pair => pair.MemberInfo)
                .OrderBy(group => group.Key);

            foreach (var group in initializationList)
            {
                OS.Log().Info("Initialization stage: {0}", Enum.GetName(typeof(InitializationStage), group.Key));

                var query = group.SelectMany(GetInitializationMethodsByType).AsParallel();

                if (query.All(ReflectionHelpers.InvokeStaticFunc<bool>))
                {
                    continue;
                }

                OS.Log().Error("Initialization failed, an initialization method returned 'false'.");
                return false;
            }
            return true;
        }

        private static IEnumerable<MetadataPair<Type, ServerModuleAttribute>> GetServerModules()
        {
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   where !assembly.GlobalAssemblyCache
                   from type in assembly.GetTypes()
                   let moduleAttribute = ReflectionHelpers.GetAttribute<ServerModuleAttribute>(type)
                   where moduleAttribute != null
                   select new MetadataPair<Type, ServerModuleAttribute>(type, moduleAttribute);
        }

        private static IEnumerable<MethodInfo> GetInitializationMethodsByType(Type type)
        {
            return type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic).
                Where(ReflectionHelpers.HasAttribute<InitializationMethodAttribute>);
        }
    }
}
