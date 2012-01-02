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
            Console.Title = "OpenStory - Account Service";

            AccountService accountService = new AccountService();

            ServiceHost host = new ServiceHost(accountService, ServerConstants.Uris.AccountService);
            host.Open();

            Log.WriteInfo("Service registered.");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
