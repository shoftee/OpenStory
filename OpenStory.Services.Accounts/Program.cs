using System;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Account
{
    internal class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Account Service";

            var accountService = new AccountService();

            ServiceHelpers.OpenServiceHost(accountService, ServiceConstants.Uris.AccountService);
            OS.Log().Info("Service registered.");

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
