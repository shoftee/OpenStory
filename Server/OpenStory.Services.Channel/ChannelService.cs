using System.ServiceModel;
using OpenStory.Framework.Contracts;
using OpenStory.Server.Channel;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Channel
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    sealed class ChannelService : GameServer
    {
        private readonly NexusConnectionInfo nexusConnectionInfo;

        public ChannelService(IServerProcess serverProcess, ChannelOperator channelOperator, NexusConnectionInfo nexusConnectionInfo)
            : base(serverProcess, channelOperator)
        {
            this.nexusConnectionInfo = nexusConnectionInfo;
        }
    }
}
