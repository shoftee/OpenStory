using System.Security.Cryptography;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;
using OpenStory.Server.Registry;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server
{
    /// <summary>
    /// Basic server module.
    /// </summary>
    public class ServerModule : NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            // No dependencies:
            Bind<IPlayerRegistry>().To<PlayerRegistry>();
            Bind<ILocationRegistry>().To<LocationRegistry>();
            Bind<IPacketScheduler>().To<PacketScheduler>();

            Bind<IEndpointProvider>().To<NetTcpEndpointProvider>();
            Bind<INexusConnectionProvider>().To<EnvironmentNexusConnectionProvider>();

            // PacketFactory requires IPacketCodeTable
            Bind<IPacketFactory>().To<PacketFactory>();

            // ServerSession requires IPacketCodeTable, ILogger
            Bind<IServerSession>().To<ServerSession>();
            Bind<IServerSessionFactory>().ToFactory();

            // IvGenerator requires RandomNumberGenerator.
            Bind<RandomNumberGenerator>().To<RNGCryptoServiceProvider>().InSingletonScope();
            Bind<IvGenerator>().ToSelf();
            
            // ServerProcess requires IServerSessionFactory, IvGenerator, ILogger
            Bind<IServerProcess>().To<ServerProcess>();

            // GameServer requires IServerConfigurator, IServerProcess, IServerOperator
            Bind<GameServiceBase>().To<GameServer>();

            // Bootstrapper requires IGenericServiceFactory, ILogger.
            Bind<Bootstrapper>().ToSelf().InSingletonScope();
        }
    }
}
