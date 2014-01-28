using System;
using System.Threading;
using Ninject;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Channel;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Channel
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
            var kernel = new StandardKernel(new ChannelServerModule(), new WcfServiceModule());

            kernel.Bind<NexusConnectionInfo>().ToConstant(GetNexusConnectionInfo());
            kernel.Bind<OsWcfConfiguration>().ToConstant(GetWcfConfiguration());

            return kernel;
        }

        private static NexusConnectionInfo GetNexusConnectionInfo()
        {
            var accessToken = new Guid("DEA61FBF-26F6-4F68-9E44-A34ABEEBDB93");
            return new NexusConnectionInfo(accessToken);
        }

        private static OsWcfConfiguration GetWcfConfiguration()
        {
            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Channel");
            var configuration = OsWcfConfiguration.For<ChannelService>(baseUri);
            return configuration;
        }
    }
}
