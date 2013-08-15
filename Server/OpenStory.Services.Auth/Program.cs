using OpenStory.Server.Auth;
using log4net.Config;
using Ninject;

namespace OpenStory.Services.Auth
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
            kernel.Load(new AuthServerModule());
            return kernel.Get<Bootstrapper>();
        }
    }
}
