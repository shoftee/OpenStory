using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Cryptography;
using OpenMaple.Networking;

namespace OpenMaple.Client
{
    // TODO: Rename to ChannelClient and move to a better namespace :/
    class Client : ClientBase
    {
        public int ChannelId { get; set; }

        public Client(ISession session)
            : base(session)
        {
        }

        public void Disconnect() { }
    }
}
