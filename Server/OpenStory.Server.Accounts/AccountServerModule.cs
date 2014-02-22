using Ninject.Modules;
using NodaTime;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Accounts
{
    /// <summary>
    /// Account service module.
    /// </summary>
    public sealed class AccountServerModule : NinjectModule
    {
        /// <inheritdoc/>
        public override void Load()
        {
            // No dependencies
            Bind<IClock>().ToConstant(SystemClock.Instance);

            // AccountServer
            // ^ IClock
            Bind<IAccountService, IRegisteredService>().To<AccountServer>().InSingletonScope();
        }
    }
}
