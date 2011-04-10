using System;
using System.ServiceModel;
using OpenStory.Server;

namespace OpenStory.AuthService
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - AuthServer";

            var authServer = new AuthServer();

            var uri = new Uri(ServerConstants.AuthServiceUri);
            var host = new ServiceHost(authServer);
            host.AddServiceEndpoint(typeof (IAuthService), ServiceHelpers.GetBinding(), uri);

            authServer.Start();
            Console.ReadLine();
        }
    }
}