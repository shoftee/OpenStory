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
            // No dependencies
            Bind<IPacketCodeTable>().To<ChannelPacketCodeTable>().InSingletonScope();

            // IGameClientFactory<ChannelClient> <= ChannelClient
            // ChannelClient <= IServerSession, IPacketFactory, ILogger (external)
            Bind<IGameClientFactory<ChannelClient>>().ToFactory();

            // ChannelOperator <= IGameClientFactory<ChannelClient>, IPlayerRegistry
            Bind<IServerOperator, IWorldToChannelRequestHandler>().To<ChannelOperator>().InSingletonScope();
        }
    }
}
