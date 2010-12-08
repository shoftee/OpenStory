using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMaple.Cryptography;
using OpenMaple.Networking;

namespace OpenMaple.Client
{
    class Client
    {
        public Session Session { get; private set; }

        public MapleAesEncryption SendCrypto { get; private set; }
        public MapleAesEncryption ReceiveCrypto { get; private set; }

        public Client(MapleAesEncryption send, MapleAesEncryption receive, Session session)
        {
            this.SendCrypto = send;
            this.ReceiveCrypto = receive;
            this.Session = session;
        }

        public void Disconnect() {}
    }
}
