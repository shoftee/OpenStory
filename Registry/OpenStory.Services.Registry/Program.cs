using OpenStory.Server.Registry;
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
            kernel.Load(new RegistryModule());
            return kernel.Get<Bootstrapper>();
        }
    }
}
