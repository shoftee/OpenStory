using System.ServiceModel;
using OpenStory.Server.Channel;
using OpenStory.Services.Wcf;

namespace OpenStory.Services.Channel
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    internal sealed class ChannelService : RegisteredServiceBase<ChannelServer>
    {
        private readonly NexusConnectionInfo nexusConnectionInfo;

        public ChannelService(ChannelServer channelServer, NexusConnectionInfo nexusConnectionInfo)
            : base(channelServer)
        {
            this.nexusConnectionInfo = nexusConnectionInfo;
        }
    }
}
