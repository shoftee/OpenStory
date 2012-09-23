using System;
using System.Threading;
using OpenStory.Server;
using OpenStory.Server.Auth.Data;
using OpenStory.Server.Fluent;
using OpenStory.Server.Modules.Logging;
using OpenStory.Services.Clients;

namespace OpenStory.Services.Auth
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Authentication Service";

            string error;
            var configuration = NexusConnectionInfo.FromCommandLine(out error);
            if (error != null)
            {
                Console.WriteLine(error);
                Console.ReadLine();
                return;
            }

            InitializeAndStart(configuration);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void InitializeAndStart(NexusConnectionInfo configuration)
        {
            var authService = new AuthService();
            var nexus = new NexusServiceClient(configuration.NexusUri);

            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services()
                    .Host(authService).Through(nexus)
                    .WithAccessToken(configuration.AccessToken)
                    .Done()
                .DataManagers()
                    .DefaultManager(new AuthDataManager()).Done();

            OS.Log().Info("Service registered.");
        }
    }
}
