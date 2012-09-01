using System;
using System.Linq;
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
        /// <param name="appDomain">The AppDomain instance to execute an assembly in.</param>
        /// <param name="module">The <see cref="OpenStoryModule"/> instance to launch.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="appDomain"/> or <paramref name="module"/> is <c>null</c>.
        /// </exception>
        /// <returns>the new thread.</returns>
        public static Thread LaunchModule(this AppDomain appDomain, OpenStoryModule module)
        {
            if (appDomain == null) throw new ArgumentNullException("appDomain"); 
            if (module == null) throw new ArgumentNullException("module");

            ThreadStart threadStart = () => appDomain.ExecuteAssemblyByName(module.AssemblyName, module.Arguments.ToArray());
            var thread = new Thread(threadStart)
                         {
                             IsBackground = false,
                         };
            thread.Start();
            return thread;
        }

        /// <summary>
        /// Gets a new AppDomain with the security priviledges of the caller and the default AppDomainSetup.
        /// </summary>
        /// <param name="friendlyName">The friendly name for the new AppDomain.</param>
        /// <returns>the new AppDomain object.</returns>
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