using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Threading;
using Ninject;
using Ninject.Extensions.Factory;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Registry
{
    class Program
    {
        public static void Main()
        {
            var kernel = new StandardKernel(new WcfServiceModule());

            kernel.Bind<WcfConfiguration>().ToConstant(GetRegistryConfiguration());
            kernel.Bind<IServiceFactory<IRegistryService>>().ToFactory();
            kernel.Bind<IRegistryService>().To<RegistryService>();
            kernel.Get<IBootstrapper>().Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static WcfConfiguration GetRegistryConfiguration()
        {
            var uri = new Uri("net.tcp://localhost/OpenStory/Registry");
            var configuration = WcfConfiguration.Create<RegistryService>(uri, ConfigureRegistryHost);
            return configuration;
        }

        private static void ConfigureRegistryHost(ServiceHost host)
        {
            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
        }
    }
}
