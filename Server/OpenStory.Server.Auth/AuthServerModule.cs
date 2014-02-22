using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Common;
using OpenStory.Framework.Contracts;
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
            Bind<IPacketCodeTable>().To<StubAuthPacketCodeTable>().InSingletonScope();
            Bind<IAccountProvider>().To<StubAccountProvider>().InSingletonScope();

            // AccountSession 
            // ^ IAccountService
            Bind<IAccountSession>().To<AccountSession>();
            
            // SimpleAuthenticator 
            // ^ IAccountProvider, IAccountService
            Bind<IAuthenticator>().To<SimpleAuthenticator>().InSingletonScope();

            // IGameClientFactory<AuthClient> 
            // ^ AuthClient 
            // ^ IAuthenticator, IServerSession, IPacketFactory, ILogger (external)
            Bind<IGameClientFactory<AuthClient>>().ToFactory();

            // AuthOperator 
            // ^ IGameClientFactory<AuthClient>
            Bind<IServerOperator>().To<AuthOperator>();

            // AuthServer 
            // ^ IServerProcess, AuthOperator
            Bind<IRegisteredService>().To<AuthServer>().InSingletonScope();
        }
    }
}
