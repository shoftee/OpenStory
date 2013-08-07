using Ninject;
using OpenStory.Server;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Account
{
    internal static class Program
    {
        private static void Main()
        {
            var bootstrapper = Initialize();
            bootstrapper.Start();
        }

        private static Bootstrapper Initialize()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IAccountService, GameServiceBase>().To<AccountService>().InSingletonScope();
            return kernel.Get<Bootstrapper>();
        }
    }
}
