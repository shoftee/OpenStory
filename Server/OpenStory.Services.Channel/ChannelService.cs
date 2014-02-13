using System.ServiceModel;
using OpenStory.Server.Channel;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Channel
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    internal sealed class ChannelService : ChannelServer
    {
        private readonly NexusConnectionInfo nexusConnectionInfo;

        public ChannelService(IServerProcess process, ChannelOperator channelOperator, IServiceContainer<IWorldToChannelRequestHandler> world, NexusConnectionInfo nexusConnectionInfo)
            : base(process, channelOperator, world)
        {
            this.nexusConnectionInfo = nexusConnectionInfo;
        }
    }
}
