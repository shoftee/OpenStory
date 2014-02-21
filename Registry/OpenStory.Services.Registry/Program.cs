using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using Ninject;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Registry
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
            var kernel = new StandardKernel(new WcfServiceModule());

            kernel.Bind<IRegistryService>().To<RegistryService>();

            var baseUri = new Uri("net.tcp://localhost:0/OpenStory/Registry");
            kernel.Bind<OsWcfConfiguration>().ToConstant(OsWcfConfiguration.For<RegistryService>(baseUri));

            InitializePresetRegistrations(kernel);

            return kernel;
        }

        private static void InitializePresetRegistrations(IKernel kernel)
        {
            kernel
                .Bind<IDictionary<Guid, OsServiceConfiguration>>()
                .ToConstant(GetPresetServiceConfigurations())
                .WhenInjectedInto<RegistryService>();
        }

        private static IDictionary<Guid, OsServiceConfiguration> GetPresetServiceConfigurations()
        {
            return new Dictionary<Guid, OsServiceConfiguration>()
                   {
                       {
                           new Guid("24BBB937-49EE-422C-A040-A42432DAFB3C"), 
                           new OsServiceConfiguration(
                               new Dictionary<string, object>()
                               {
                                   { "Name", "Account-Server" },
                               })
                       },
                       {
                           new Guid("18B87A4B-E405-43F4-A1C2-A0AED35E3E15"), 
                           new OsServiceConfiguration(
                               new Dictionary<string, object>()
                               {
                                   { "Name", "Auth-Server" },
                                   { "Endpoint", new IPEndPoint(IPAddress.Loopback, 8484) },
                               })
                       },
                       {
                           new Guid("DEA61FBF-26F6-4F68-9E44-A34ABEEBDB93"),
                           new OsServiceConfiguration(
                               new Dictionary<string, object>()
                               {
                                   { "Name", "Channel-Server-1" },
                                   { "Endpoint", new IPEndPoint(IPAddress.Loopback, 8586) },
                               })
                       },
                   };
        }
    }
}
