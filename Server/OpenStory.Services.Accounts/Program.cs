using System;
using System.Threading;
using OpenStory.Common.Tools;
using OpenStory.Server.Diagnostics;
using OpenStory.Server.Fluent;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = "OpenStory - Account Service";

            string error;
            var parameters = ParameterList.FromEnvironment(out error);
            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }

            Initialize(parameters);

            var service = OS.Svc().Local();

            ServiceHelpers.OpenServiceHost(service, ServiceConstants.Uris.AccountService);
            OS.Log().Info("Service registered.");

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Initialize(ParameterList parameters)
        {
            var accountService = new AccountService();

            OS.Initialize()
                .Logger(new ConsoleLogger())
                .Services().WithLocal(accountService).Done();
        }
    }
}
