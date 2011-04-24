using System;
using System.ServiceModel;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.Server;
using OpenStory.ServiceModel;

namespace OpenStory.AuthService
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - AuthServer";

            var authServer = new AuthServer();

            var host = new ServiceHost(authServer);
            host.AddServiceEndpoint(typeof(IAuthService), ServiceHelpers.GetPipeBinding(), ServerConstants.AuthServiceUri);
            
            host.Open();
            Log.WriteInfo("Service registered.");
            
            authServer.Start();
            
            Thread.Sleep(Timeout.Infinite);
        }
    }
}