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
        /// <param name="info">The information for the handshake process.</param>
        public void Start(RollingIvFactory factory, HandshakeInfo info)
        {
            this.ThrowIfNoPacketReceivedSubscriber();

            this.Crypto = ServerCrypto.New(factory, info.ClientIv, info.ServerIv);

            byte[] helloPacket = ConstructHandshakePacket(info);
            this.Session.Start();
            this.Session.Write(helloPacket);
        }

        #region Outgoing logic

        private static byte[] ConstructHandshakePacket(HandshakeInfo info)
        {
            using (var builder = new PacketBuilder(16))
            {
                builder.WriteInt16(info.Header);
                builder.WriteInt16(info.Version);
                builder.WriteLengthString(info.SubVersion);
                builder.WriteBytes(info.ClientIv);
                builder.WriteBytes(info.ServerIv);

                // Server ID (used for localizations and test servers)
                builder.WriteByte(info.ServerId);

                return builder.ToByteArray();
            }
        }

        #endregion
    }
}
