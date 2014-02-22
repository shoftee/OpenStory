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
            Bind<IClock>().ToMethod(ctx => SystemClock.Instance).InSingletonScope();
            Bind<IAccountService, IRegisteredService>().To<AccountServer>();
        }
    }
}
