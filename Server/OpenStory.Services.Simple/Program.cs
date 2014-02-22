using System.Threading;
using Ninject;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Simple
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
            var kernel = new StandardKernel();
            kernel.Bind<IBootstrapper>().To<SimpleBootstrapper>();
            return kernel;
        }
    }
}
