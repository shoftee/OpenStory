using OpenStory.Services.Contracts;
using log4net.Config;
using Ninject;

namespace OpenStory.Services.Registry
{
    internal static class Program
    {
        private static void Main()
        {
            XmlConfigurator.Configure();
            var bootstrapper = Initialize();
            bootstrapper.Start();
        }

        private static Bootstrapper Initialize()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IGenericServiceFactory>().To<RegistryServiceFactory>();
            return kernel.Get<Bootstrapper>();
        }
    }
}
