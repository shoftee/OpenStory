using System;
using System.Threading;
using OpenStory.Server;
using OpenStory.Server.Auth.Data;
using OpenStory.Server.Fluent;
using OpenStory.Server.Modules.Logging;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Auth
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Authentication Service";

            string error;
            var info = NexusConnectionInfo.FromCommandLine(out error);
            if (error != null)
            {
                Console.WriteLine(error);
                Console.ReadLine();
                return;
            }

            ServiceConfiguration configuration;
            var result = GetServiceConfiguration(info, out configuration);

            var success = ServiceHelpers.ProcessGetConfigurationResult(result, out error);
            if (!success)
            {
                Console.WriteLine(error);
                Console.ReadLine();
                return;
            }

            var service = new AuthService();

            if (!service.Configure(configuration, out error))
            {
                Console.WriteLine(error);
                Console.ReadLine();
                return;
            }

            InitializeAndStart(service);
            var uriString = configuration["ServiceUri"];

            var host = ServiceHelpers.OpenServiceHost(service, new Uri(uriString));

            OS.Log().Info("Service registered.");
            using (host)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static ServiceOperationResult GetServiceConfiguration(NexusConnectionInfo info, out ServiceConfiguration configuration)
        {
            using (var nexus = new NexusServiceClient(info.NexusUri))
            {
                ServiceOperationResult result = nexus.TryGetServiceConfiguration(info.AccessToken, out configuration);
                return result;
            }
        }

        private static void InitializeAndStart(IGameService service)
        {
            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services()
                    .Host(service).Done()
                .DataManagers()
                    .DefaultManager(new AuthDataManager()).Done();
        }
    }
}
