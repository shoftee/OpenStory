using System;
using System.Threading;
using OpenStory.Server.Accounts;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;
using Ninject;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        public static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            
            CreateKernel().Get<IBootstrapper>().Start();
            Thread.Sleep(Timeout.Infinite);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new AccountServerModule(), new WcfServiceModule());

            kernel.Bind<NexusConnectionInfo>().ToConstant(GetNexusConnectionInfo());
            kernel.Bind<OsWcfConfiguration>().ToConstant(GetWcfConfiguration());
         
            return kernel;
        }

        private static NexusConnectionInfo GetNexusConnectionInfo()
        {
            var accessToken = new Guid("24BBB937-49EE-422C-A040-A42432DAFB3C");
            return new NexusConnectionInfo(accessToken);
        }

        private static OsWcfConfiguration GetWcfConfiguration()
        {
            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Account");
            var configuration = OsWcfConfiguration.For<AccountService>(baseUri);
            return configuration;
        }
    }
}
