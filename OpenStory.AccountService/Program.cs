using System;
using System.ServiceModel;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.ServiceModel;

namespace OpenStory.AccountService
{
    class Program
    {
        static void Main()
        {
            Console.Title = "OpenStory - AccountService";

            AccountService accountService = new AccountService();

            ServiceHost host = new ServiceHost(accountService);
            host.AddServiceEndpoint(typeof(IAccountService), ServiceHelpers.GetBinding(), ServerConstants.AccountServiceUri);

            host.Open();
            Log.WriteInfo("Service registered.");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
