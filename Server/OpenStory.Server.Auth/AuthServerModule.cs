using Ninject.Extensions.Factory;
using OpenStory.Common;
using OpenStory.Server.Processing;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Auth
{
    /// <summary>
    /// Auth server module.
    /// </summary>
    public sealed class AuthServerModule : ServerModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            base.Load();

            Bind<IAccountSession>().To<AccountSession>();
            Bind<IAuthenticator>().To<SimpleAuthenticator>();

            Bind<IPacketCodeTable>().To<AuthServerPackets>();

            Bind<IClientFactory<AuthClient>>().ToFactory();

            Bind<IServerOperator, IAuthServer>().To<AuthOperator>();
            Bind<IAuthService, GameServiceBase>().To<AuthService>();
            Bind<IGenericServiceFactory>().To<DiscoverableServiceFactory<IAuthService>>();
        }
    }
}
