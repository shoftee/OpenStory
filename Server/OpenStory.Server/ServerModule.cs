using System.Security.Cryptography;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Networking;
using OpenStory.Server.Processing;
using OpenStory.Server.Registry;
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
            // No dependencies
            Bind<IRollingIvFactoryProvider>().To<EmsRollingIvFactoryProvider>().InSingletonScope();
            Bind<ILocationRegistry>().To<LocationRegistry>();
            Bind<IPacketScheduler>().To<PacketScheduler>();

            // PacketFactory <= IPacketCodeTable (external)
            Bind<IPacketFactory>().To<PacketFactory>();

            // IvGenerator <= RandomNumberGenerator
            Bind<RandomNumberGenerator>().To<RNGCryptoServiceProvider>();
            Bind<IvGenerator>().ToSelf();

            // ServerSession <= IPacketCodeTable (external)
            Bind<IServerSession>().To<ServerSession>();
            
            // IServerSessionFactory <= IServerSession (external)
            // ServerProcess <= ISocketAcceptorFactory, IServerSessionFactory, IvGenerator, ILogger (external)
            Bind<ISocketAcceptorFactory>().ToFactory();
            Bind<IServerSessionFactory>().ToFactory();
            Bind<IServerProcess>().To<ServerProcess>();
        }
    }
}
