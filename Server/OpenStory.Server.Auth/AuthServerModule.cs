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
            Bind<IAccountSession>().To<AccountSession>();
            Bind<IAuthenticator>().To<SimpleAuthenticator>().InSingletonScope();

            Bind<IPacketCodeTable>().To<AuthPacketCodeTable>().InSingletonScope();

            Bind<IGameClientFactory<AuthClient>>().ToFactory();

            Bind<IServerOperator>().To<AuthOperator>();
            Bind<IServiceFactory<IAuthService>>().ToFactory();
        }
    }
}
