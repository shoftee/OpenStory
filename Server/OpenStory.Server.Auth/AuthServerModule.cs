using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Auth server module.
    /// </summary>
    public sealed class AuthServerModule : NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            // No dependencies
            Bind<IPacketCodeTable>().To<AuthPacketCodeTable>().InSingletonScope();

            // AccountSession <= IAccountService
            Bind<IAccountSession>().To<AccountSession>();

            // SimpleAuthenticator <= IAccountProvider, IAccountService
            Bind<IAuthenticator>().To<SimpleAuthenticator>().InSingletonScope();

            // AuthClient <= IAuthenticator, IServerSession, IPacketFactory, ILogger (external)
            Bind<IGameClientFactory<AuthClient>>().ToFactory();

            // AuthOperator <= IGameClientFactory<AuthClient>
            Bind<IServerOperator>().To<AuthOperator>().InSingletonScope();
        }
    }
}
