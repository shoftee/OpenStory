using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Cryptography;
using OpenStory.Networking;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents an encrypted network session.
    /// </summary>
    public sealed class ServerSession : EncryptedNetworkSession
    {
        private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

        #region Fields and properties

        /// <summary>
        /// A unique 32-bit network session identifier.
        /// </summary>
        /// <remarks>
        /// This session identifier and the account session identifier are different things.
        /// </remarks>
        public int NetworkSessionId { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of <see cref="ServerSession"/>
        /// </summary>
        public ServerSession()
        {
            this.NetworkSessionId = RollingSessionId.Increment();
        }

        /// <summary>
        /// Initiates the session operations.
        /// </summary>
        /// <param name="factory">The <see cref="RollingIvFactory"/> to use to create <see cref="RollingIv"/> instances.</param>
        /// <param name="clientIv">The client IV to use for the cryptographic transformation.</param>
        /// <param name="serverIv">The server IV to use for the cryptographic transformation.</param>
        public void Start(RollingIvFactory factory, byte[] clientIv, byte[] serverIv)
        {
            this.ThrowIfNoPacketReceivedSubscriber();

            this.Crypto = ServerCrypto.New(factory, clientIv, serverIv);

            byte[] helloPacket = ConstructHelloPacket(clientIv, serverIv, factory.Version);
            this.Session.Start();
            this.Session.Write(helloPacket);
        }

        #region Outgoing logic

        private static byte[] ConstructHelloPacket(byte[] clientIv, byte[] serverIv, ushort version)
        {
            using (var builder = new PacketBuilder(16))
            {
                builder.WriteInt16(0x0E);
                builder.WriteInt16(version);
                builder.WriteLengthString("2"); // supposedly some patch thing?
                builder.WriteBytes(clientIv);
                builder.WriteBytes(serverIv);

                // Test server flag.
                builder.WriteByte(0x05);

                return builder.ToByteArray();
            }
        }

        #endregion
    }
}
