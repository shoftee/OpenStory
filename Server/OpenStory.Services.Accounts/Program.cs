using System;
using System.Threading;
using OpenStory.Server.Diagnostics;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private static void Main()
        {
            Initialize();

            Console.Title = "OpenStory - Account Service";

            var service = OS.Svc().Local();

            ServiceHelpers.OpenServiceHost(service, ServiceConstants.Uris.AccountService);
            OS.Log().Info("Service registered.");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Initialize()
        {
            var accountService = new AccountService();

            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services().WithLocal(accountService).Done();
        }
    }
}
