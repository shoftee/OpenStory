using Ninject.Extensions.Factory;
using Ninject.Modules;
using OpenStory.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Channel server module.
    /// </summary>
    public class ChannelServerModule : NinjectModule
    {
        /// <inheritdoc />
        public override void Load()
        {
            Bind<IPacketCodeTable>().To<ChannelPacketCodeTable>().InSingletonScope();

            Bind<IGameClientFactory<ChannelClient>>().ToFactory();

            Bind<IServerOperator>().To<ChannelOperator>();

            Bind<IWorldChannelRequestHandler>().To<ChannelServer>();
            Bind<IServiceFactory<IWorldChannelRequestHandler>>().ToFactory();
        }
    }
}
