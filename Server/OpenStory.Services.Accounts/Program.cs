using System;
using System.Threading;
using log4net.Config;
using Ninject;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        public static void Main()
        {
            XmlConfigurator.Configure();

            var kernel = CreateKernel();
            kernel.Get<IBootstrapper>().Start();
            Thread.Sleep(Timeout.Infinite);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new WcfServiceModule());

            kernel.Bind<NexusConnectionInfo>().ToConstant(GetNexusConnectionInfo());
            kernel.Bind<RegisteredServiceBase>().To<AccountService>();

            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Account");
            kernel.Bind<WcfConfiguration>().ToConstant(WcfConfiguration.For<AccountService>(baseUri));
            
            return kernel;
        }

        private static NexusConnectionInfo GetNexusConnectionInfo()
        {
            var accessToken = new Guid("24BBB937-49EE-422C-A040-A42432DAFB3C");
            return new NexusConnectionInfo(accessToken);
        }
    }
}
