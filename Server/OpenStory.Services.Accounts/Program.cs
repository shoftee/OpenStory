using System;
using System.Threading;
using OpenStory.Server;
using OpenStory.Server.Diagnostics;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Account Service";

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
            var service = new AccountService();
            var nexusFragment = new AccountNexusFragment(configuration.NexusUri);

            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services()
                    .Host(service, nexusFragment)
                    .WithAccessToken(configuration.AccessToken)
                    .Done();

            OS.Log().Info("Service registered.");
        }
    }
}
