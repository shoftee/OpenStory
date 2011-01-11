using System;
using OpenMaple.Data;
using OpenMaple.Game;
using OpenMaple.Networking;

namespace OpenMaple.Server
{
    class ChannelClient : AbstractClient
    {
        private SimpleAccount account;

        public override IAccount AccountInfo
        {
            get { return this.account; }
        }

        public ChannelClient(NetworkSession networkSession)
            : base(networkSession)
        {
        }
    }

    class SimpleAccount : IAccount
    {
        public int AccountId { get { throw new NotImplementedException(); } }

        public string UserName { get { throw new NotImplementedException(); } }

        public Gender Gender { get { throw new NotImplementedException(); } }

        public GameMasterLevel GameMasterLevel { get { throw new NotImplementedException(); } }
    }
}
