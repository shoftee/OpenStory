using System;
using System.Threading;
using OpenStory.Server.Auth;
using OpenStory.Server.Auth.Data;
using OpenStory.Server.Diagnostics;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Auth
{
    internal static class Program
    {
        private static void Main()
        {
            Initialize();

            Console.Title = "OpenStory - Authentication Service";

            var service = OS.Svc().Local();

            ServiceHelpers.OpenServiceHost(service, ServiceConstants.Uris.AuthService);
            OS.Log().Info("Service registered.");

            service.Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Initialize()
        {
            var authService = new AuthService();

            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services().WithLocal(authService).Done()
                .DataManagers().DefaultManager(new AuthDataManager()).Done();
        }
    }
}
