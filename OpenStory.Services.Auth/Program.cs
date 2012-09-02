using System;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.Server.Auth;
using OpenStory.Server.Auth.Data;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Auth
{
    internal class Program
    {
        private static void Main()
        {
            OS.Initialize()
                .DataManagers().DefaultManager(new AuthDataManager()).Done();

            Console.Title = "OpenStory - Authentication Service";

            var authServer = new AuthServer();

            ServiceHelpers.OpenServiceHost(authServer, ServiceConstants.Uris.AuthService);
            OS.Log().Info("Service registered.");

            authServer.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
