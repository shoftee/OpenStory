using System;
using System.Threading;
using Ninject;
using OpenStory.Server;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private const string Title = @"OpenStory - Account Service";
        private const string TitleStatusFormat = @"OpenStory - Account Service - {0}";

        private static void Main()
        {
            Console.Title = Title;

            var bootstrapper = Initialize();

            try
            {
                using (bootstrapper.Service())
                {
                    Console.Title = string.Format(TitleStatusFormat, "Running");
                    Thread.Sleep(Timeout.Infinite);
                }
            }
            catch (BootstrapException exception)
            {
                Console.Title = string.Format(TitleStatusFormat, "Error");
                Console.WriteLine(exception);
                Console.ReadLine();
            }
        }

        private static Bootstrapper Initialize()
        {
            var kernel = new StandardKernel();
            
            kernel.Bind<IAccountService, GameServiceBase>().To<AccountService>().InSingletonScope();

            return kernel.Get<Bootstrapper>();
        }
    }
}
