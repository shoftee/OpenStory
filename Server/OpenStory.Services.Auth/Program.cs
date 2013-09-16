using System;
using System.Threading;
using log4net.Config;
using Ninject;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Auth;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Auth
{
    static class Program 
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
            var kernel = new StandardKernel(new AuthServerModule(), new WcfServiceModule());

            kernel.Bind<NexusConnectionInfo>().ToConstant(GetNexusConnectionInfo());
            kernel.Bind<RegisteredServiceBase>().To<AuthService>();

            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Auth");
            kernel.Bind<WcfConfiguration>().ToConstant(WcfConfiguration.For<AuthService>(baseUri));

            return kernel;
        }

        private static NexusConnectionInfo GetNexusConnectionInfo()
        {
            var accessToken = new Guid("18B87A4B-E405-43F4-A1C2-A0AED35E3E15");
            return new NexusConnectionInfo(accessToken);
        }
    }
}
