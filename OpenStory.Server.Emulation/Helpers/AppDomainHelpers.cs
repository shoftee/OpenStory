using System;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using OpenStory.Common.Tools;

namespace OpenStory.Server.Emulation.Helpers
{
    internal static class AppDomainHelpers
    {
        private static readonly AppDomainSetup DefaultAppDomainSetup =
            new AppDomainSetup
            {
                ApplicationName = "OpenStory-ServerEmulator",
                LoaderOptimization = LoaderOptimization.NotSpecified,
            };

        /// <summary>
        /// Creates a new thread and executes the specified assembly into the AppDomain instance, passing the specified arguments.
        /// </summary>
        /// <param name="instance">The AppDomain instance to execute an assembly in.</param>
        /// <param name="assemblyName">The AssemblyName instance for the assembly to execute.</param>
        /// <param name="args">The arguments to pass to the entry point method.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="assemblyName"/> is <c>null</c>.</exception>
        /// <returns>The new thread.</returns>
        public static Thread LaunchAssembly(this AppDomain instance, AssemblyName assemblyName, params string[] args)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException("assemblyName");
            }
            var thread = new Thread(() => instance.ExecuteAssemblyByName(assemblyName, args));
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Gets a new AppDomain with the security priviledges of the caller and the default AppDomainSetup.
        /// </summary>
        /// <param name="friendlyName">The friendly name for the new AppDomain.</param>
        /// <returns>The new AppDomain object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="friendlyName" /> is <c>null</c> or empty.</exception>
        public static AppDomain GetNewDomain(string friendlyName)
        {
            if (String.IsNullOrEmpty(friendlyName)) throw new ArgumentNullException("friendlyName");

            Evidence evidence = AppDomain.CurrentDomain.Evidence;
            AppDomain newDomain = AppDomain.CreateDomain(friendlyName, evidence, DefaultAppDomainSetup);

            newDomain.UnhandledException += UnhandledExceptionHandler;
            newDomain.DomainUnload += DomainUnloadHandler;

            return newDomain;
        }

        private static void DomainUnloadHandler(object sender, EventArgs e)
        {
            var domain = sender as AppDomain;
            if (domain == null)
            {
                return;
            }

            Log.WriteInfo("[{0}] Domain unloaded.", domain.FriendlyName);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var domain = sender as AppDomain;
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
                Log.WriteError("[{0}] Fatal unhandled exception: {1} ", domain.FriendlyName,
                               args.ExceptionObject.ToString());
                Log.WriteError("The process will now terminate.");
                Console.ReadLine();
            }
        }
    }
}