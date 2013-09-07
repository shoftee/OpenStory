using log4net.Config;
using Ninject;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private static void Main()
        {
            XmlConfigurator.Configure();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IAccountService, RegisteredServiceBase>().To<AccountService>();
            return kernel;
        }
    }
}
