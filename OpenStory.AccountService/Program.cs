using System;
using System.ServiceModel;
using OpenStory.Server;

namespace OpenStory.AccountService
{
    class Program
    {
        static void Main()
        {
            Console.Title = "OpenStory - AccountService";

            AccountService accountService = new AccountService();

            Uri uri = new Uri(ServerConstants.AuthServiceUri);
            ServiceHost host = new ServiceHost(accountService);
            host.AddServiceEndpoint(typeof(IAccountService), ServiceHelpers.GetBinding(), uri);

            host.Open();
            Console.WriteLine("AccountService started.");
            Console.ReadLine();
        }
    }
}
