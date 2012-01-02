using System;
using System.ServiceModel;
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

            var host = new ServiceHost(authServer, ServerConstants.Uris.AuthService);
            host.Open();
            
            Log.WriteInfo("Service registered.");
            
            authServer.Start();
            
            Thread.Sleep(Timeout.Infinite);
        }
    }
}