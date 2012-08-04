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

            var accountService = new AccountService();

            ServiceHelpers.OpenServiceHost(accountService, ServerConstants.Uris.AccountService);
            Log.WriteInfo("Service registered.");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
