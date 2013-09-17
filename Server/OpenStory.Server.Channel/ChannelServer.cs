using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    class ChannelServer : GameServer, IWorldChannelRequestHandler
    {
        private readonly ChannelOperator channelOperator;

        public ChannelServer(IServerProcess serverProcess, ChannelOperator channelOperator)
            : base(serverProcess, channelOperator)
        {
            this.channelOperator = channelOperator;
        }

        public void BroadcastIntoChannel(CharacterKey[] targets, byte[] data)
        {
            this.channelOperator.BroadcastIntoChannel(targets, data);
        }
    }
}
