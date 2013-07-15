using System;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;

namespace OpenStory.Redirector.Connection
{
    internal sealed class ClientSession : EncryptedNetworkSession
    {
        /// <summary>
        /// The event is raised after the hello packet is processed.
        /// </summary>
        public event EventHandler<HandshakeReceivedEventArgs> HandshakeReceived;

        private bool receivedHelloPacket;

        #region Constructors and instance construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSession"/> class.
        /// </summary>
        public ClientSession()
        {
            this.receivedHelloPacket = false;
        }

        #endregion

        /// <summary>
        /// Initiates the session operations.
        /// </summary>
        public void Start()
        {
            this.ThrowIfNoPacketReceivedSubscriber();

            if (this.HandshakeReceived == null)
            {
                throw new InvalidOperationException("'HandshakeReceived' has no subscribers.");
            }

            // Make space for the hello packet.
            this.PacketBuffer.Reset(32);

            this.Session.Start();
        }

        #region Incoming logic

        protected override void OnDataArrived(object sender, DataArrivedEventArgs args)
        {
            byte[] data = args.Data;
            if (!this.receivedHelloPacket)
            {
                var info = this.HandleHandshakeData(data);
                if (info == null)
                {
                    Logger.Write(LogMessageType.Error, "Handshake expected, closing session.");
                    this.Close();
                }
            }
            else
            {
                base.OnDataArrived(sender, args);
            }
        }

        private HandshakeInfo HandleHandshakeData(byte[] data)
        {
            this.PacketBuffer.AppendFill(data, 0, data.Length);

            byte[] helloPacket = this.PacketBuffer.ExtractAndReset(0);

            var reader = new PacketReader(helloPacket);

            var info = new PackagedHandshakeInfo();
            if (!info.TryParse(reader))
            {
                this.Close();
                return null;
            }

            var factory = Helpers.GetFactoryForVersion(info.Version);
            this.Crypto = ClientCrypto.New(factory, info.ClientIv, info.ServerIv);

            this.receivedHelloPacket = true;

            Logger.Write(
                LogMessageType.Connection,
                "Received handshake. Version {0}-{1}, CIV {2} SIV {3}",
                info.Version, 
                info.Subversion,
                info.ClientIv.ToHex(hyphenate: true),
                info.ServerIv.ToHex(hyphenate: true));

            this.HandshakeReceived(this, new HandshakeReceivedEventArgs(info));

            return info;
        }

        #endregion
    }
}
