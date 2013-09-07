using System;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Threading;
using Ninject;
using OpenStory.Server.Auth;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Auth
{
    static class Program 
    {
        public static void Main()
        {
            var kernel = new StandardKernel(new AuthServerModule(), new WcfServiceModule());

            kernel.Bind<IAuthService, RegisteredServiceBase>().To<AuthService>();
            kernel.Bind<WcfConfiguration>().ToConstant(GetAuthConfiguration());
            kernel.Get<IBootstrapper>().Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static WcfConfiguration GetAuthConfiguration()
        {
            var uri = new Uri("net.tcp://localhost/OpenStory/Auth");
            var configuration = WcfConfiguration.Create<AuthService>(uri, ConfigureAuthHost);
            return configuration;
        }

        private static void ConfigureAuthHost(ServiceHost host)
        {
            host.Description.Behaviors.Add(new ServiceDiscoveryBehavior());
            host.AddServiceEndpoint(new UdpDiscoveryEndpoint());
        }
    }
}
