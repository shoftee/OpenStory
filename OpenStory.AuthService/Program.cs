using System;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.ServiceModel;

namespace OpenStory.AuthService
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Authentication Service";

            var authServer = new AuthServer();

            ServiceHelpers.OpenServiceHost(authServer, ServerConstants.Uris.AuthService);
            Log.WriteInfo("Service registered.");

            authServer.Start();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}