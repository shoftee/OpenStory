using System;
using System.Threading;
using OpenStory.Server;
using OpenStory.Server.Auth.Data;
using OpenStory.Server.Diagnostics;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Auth
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Authentication Service";

            string error;
            var configuration = ServiceConfiguration.FromCommandLine(out error);
            if (error != null)
            {
                Console.WriteLine(error);
                Console.ReadLine();
                return;
            }

            InitializeAndStart(configuration);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void InitializeAndStart(ServiceConfiguration configuration)
        {
            var authService = new AuthService();
            var nexusFragment = new AuthNexusFragment(configuration.NexusUri);

            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services()
                    .Host(authService, nexusFragment)
                    .WithAccessToken(configuration.AccessToken)
                    .Done()
                .DataManagers()
                    .DefaultManager(new AuthDataManager()).Done();

            OS.Log().Info("Service registered.");
        }
    }
}
