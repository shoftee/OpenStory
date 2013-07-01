using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Ninject;
using OpenStory.Framework.Contracts;
using OpenStory.Server;
using OpenStory.Server.Fluent;
using OpenStory.Server.Modules.Logging;
using OpenStory.Services.Account;
using OpenStory.Services.Auth;
using OpenStory.Services.Contracts;
using OpenStory.Services.Registry;

namespace OpenStory.IntegrationTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = Initialize();

            var nexusUri = new Uri("net.tcp://localhost/OpenStory/Registry");

            var registry = kernel.Get<RegistryService>();
            var host = new ServiceHost(registry, nexusUri);
            host.Open();

            IRegistryService registryClient = new RegistryServiceClient(nexusUri);

            var accountUri = new Uri("net.tcp://localhost/OpenStory/Account");
            var accountConfig = ServiceConfiguration.Account(accountUri);
            var accountGuid = registryClient.RegisterService(accountConfig).Result;
            var accountInfo = new NexusConnectionInfo(accountGuid, nexusUri);

            string error;
            Bootstrap.Service<AccountService>(kernel, accountInfo, out error);
            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }

            Console.WriteLine("Woot!");
            Console.ReadLine();
        }

        private static IKernel Initialize()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ILogger>().To<ConsoleLogger>().InSingletonScope();
            kernel.Bind<IAccountService>().To<AccountService>().InSingletonScope();
            kernel.Bind<IRegistryService>().To<RegistryService>().InSingletonScope();

            OS.Initialize(kernel);

            return kernel;
        }

    }
}
