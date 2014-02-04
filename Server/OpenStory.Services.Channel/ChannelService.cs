using System.ServiceModel;
using OpenStory.Server.Channel;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Channel
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    internal sealed class ChannelService : GameServer
    {
        private readonly NexusConnectionInfo nexusConnectionInfo;

        public ChannelService(IServerProcess serverProcess, ChannelOperator channelOperator, NexusConnectionInfo nexusConnectionInfo)
            : base(serverProcess, channelOperator)
        {
            this.nexusConnectionInfo = nexusConnectionInfo;
        }
    }
}
