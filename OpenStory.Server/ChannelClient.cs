using OpenStory.Server.Networking;

namespace OpenStory.Server
{
    class ChannelClient : AbstractClient
    {
        private SimpleAccount account;

        public ChannelClient(NetworkSession networkSession)
            : base(networkSession) {}

        public override IAccount AccountInfo
        {
            get { return this.account; }
        }
    }
}