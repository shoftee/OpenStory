using System;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.Server.Auth;
using OpenStory.Server.Fluent;
using OpenStory.Server.Modules.Default;

namespace OpenStory.Services.Auth
{
    internal class Program
    {
        private static void Main()
        {
            OS.Initialize()
                .DataManager().DefaultManager(new DefaultDataManager());

            Console.Title = "OpenStory - Authentication Service";

            var authServer = new AuthServer();

            ServiceHelpers.OpenServiceHost(authServer, ServiceConstants.Uris.AuthService);
            Log.WriteInfo("Service registered.");

            authServer.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}