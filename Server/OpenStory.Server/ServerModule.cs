using System.Security.Cryptography;
using System.ServiceModel.Discovery;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Processing;
using OpenStory.Server.Registry;
using OpenStory.Services;

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
            Bind<INexusConnectionProvider>().To<EnvironmentNexusConnectionProvider>();

            // PacketFactory <= IPacketCodeTable
            Bind<IPacketFactory>().To<PacketFactory>();

            // DiscoveryEndpointProvider <= DiscoveryEndpoint
            // DefaultServiceConfigurationProvider <= DiscoveryEndpointProvider
            Bind<DiscoveryEndpoint>().To<UdpDiscoveryEndpoint>();
            Bind<DiscoveryEndpointProvider>().ToFactory();
            Bind<IServiceConfigurationProvider>().To<DefaultServiceConfigurationProvider>();
            
            // IvGenerator <= RandomNumberGenerator.
            Bind<RandomNumberGenerator>().To<RNGCryptoServiceProvider>().InSingletonScope();
            Bind<IvGenerator>().ToSelf();

            // ServerSession <= IPacketCodeTable, ILogger
            // IServerSessionFactory <= IServerSession
            // ServerProcess <= IServerSessionFactory, IvGenerator, ILogger
            Bind<IServerSession>().To<ServerSession>();
            Bind<IServerSessionFactory>().ToFactory();
            Bind<IServerProcess>().To<ServerProcess>();

            // GameServer <= IServerProcess, IServerOperator
            Bind<GameServiceBase>().To<GameServer>();

            // Bootstrapper <= IGenericServiceFactory, ILogger.
            Bind<Bootstrapper>().ToSelf().InSingletonScope();
        }
    }
}
