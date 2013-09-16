using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using log4net.Config;
using Ninject;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Registry
{
    class Program
    {
        public static void Main()
        {
            XmlConfigurator.Configure();

            var kernel = CreateKernel();

            InitializePresetRegistrations(kernel);

            kernel.Get<IBootstrapper>().Start();

            Thread.Sleep(Timeout.Infinite);
        }

        private static void InitializePresetRegistrations(IKernel kernel)
        {
            kernel
                .Bind<IDictionary<Guid, ServiceConfiguration>>()
                .ToConstant(GetPresetServiceConfigurations())
                .WhenInjectedInto<RegistryService>();
        }

        private static IDictionary<Guid, ServiceConfiguration> GetPresetServiceConfigurations()
        {
            return new Dictionary<Guid, ServiceConfiguration>()
                   {
                       {
                           new Guid("18B87A4B-E405-43F4-A1C2-A0AED35E3E15"), 
                           new ServiceConfiguration(
                               new Dictionary<string, object>()
                               {
                                   { "Endpoint", new IPEndPoint(IPAddress.Loopback, 8484)}
                               })
                       },
                       {
                           new Guid("24BBB937-49EE-422C-A040-A42432DAFB3C"), 
                           new ServiceConfiguration(new Dictionary<string, object>())
                       },
                   };
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel(new WcfServiceModule());

            kernel.Bind<IRegistryService>().To<RegistryService>();

            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Registry");
            kernel.Bind<WcfConfiguration>().ToConstant(WcfConfiguration.For<RegistryService>(baseUri));

            return kernel;
        }
    }
}
