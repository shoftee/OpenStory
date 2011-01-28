using System;
using System.Security.Policy;
using OpenStory.Common.Tools;

namespace OpenStory.Emulation.Helpers
{
    class AppDomainHelpers
    {
        private static readonly AppDomainSetup DefaultAppDomainSetup =
            new AppDomainSetup
            {
                ApplicationName = "OpenStory-ServerEmulator",
                LoaderOptimization = LoaderOptimization.SingleDomain
            };

        /// <summary>
        /// Gets a new AppDomain with the security priviledges of the caller and the default AppDomainSetup.
        /// </summary>
        /// <param name="friendlyName">The friendly name for the new AppDomain.</param>
        /// <returns>The new AppDomain object.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="friendlyName" /> is <c>null</c>.</exception>
        public static AppDomain GetNewDomain(string friendlyName)
        {
            if (friendlyName == null) throw new ArgumentNullException("friendlyName");

            Evidence evidence = AppDomain.CurrentDomain.Evidence;
            AppDomain newDomain = AppDomain.CreateDomain(friendlyName, evidence, DefaultAppDomainSetup);

            newDomain.UnhandledException += UnhandledExceptionHandler;
            newDomain.DomainUnload += DomainUnloadHandler;

            return newDomain;
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
                Console.ReadLine();
            }
        }

    }
}
