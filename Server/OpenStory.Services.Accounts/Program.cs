using System;
using System.Threading;
using Ninject;
using OpenStory.Server;
using OpenStory.Server.Fluent;
using OpenStory.Server.Modules.Logging;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Title = @"OpenStory - Account Service";

            var kernel = Initialize();

            string error;
            var service = Bootstrap.Service<AccountService>(kernel, out error);
            if (error != null)
            {
                Console.Title = @"OpenStory - Account Service - Error";
                
                Console.WriteLine(error);
                Console.ReadLine();
                return;
            }

            using (service)
            {
                Console.Title = @"OpenStory - Account Service - Running";
                
                Thread.Sleep(Timeout.Infinite);
            }
        }

        private static IKernel Initialize()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ILogger>().ToConstant(new ConsoleLogger()).InSingletonScope();
            kernel.Bind<AccountService>().ToConstant(new AccountService());

            OS.Initialize(kernel);

            return kernel;
        }
    }
}
