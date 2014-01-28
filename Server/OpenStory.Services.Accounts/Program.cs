using System;
using System.Threading;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;
using Ninject;
using NodaTime;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        public static void Main()
        {
            CreateKernel().Get<IBootstrapper>().Start();
            Thread.Sleep(Timeout.Infinite);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new WcfServiceModule());

            kernel.Bind<IClock>().ToMethod(ctx => SystemClock.Instance).InSingletonScope();
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
