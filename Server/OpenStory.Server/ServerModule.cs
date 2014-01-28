using System.Security.Cryptography;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Networking;
using OpenStory.Server.Processing;
using OpenStory.Server.Registry;

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

            // PacketFactory <= IPacketCodeTable
            Bind<IPacketFactory>().To<PacketFactory>();

            // IvGenerator <= RandomNumberGenerator.
            Bind<RandomNumberGenerator>().To<RNGCryptoServiceProvider>().InSingletonScope();
            Bind<IvGenerator>().ToSelf();

            // ServerSession <= IPacketCodeTable
            Bind<IServerSession>().To<ServerSession>();
            
            // IServerSessionFactory <= IServerSession
            // ServerProcess <= ISocketAcceptorFactory, IServerSessionFactory, IvGenerator, ILogger
            Bind<ISocketAcceptorFactory>().ToFactory();
            Bind<IServerSessionFactory>().ToFactory();
            Bind<IServerProcess>().To<ServerProcess>();
        }
    }
}
