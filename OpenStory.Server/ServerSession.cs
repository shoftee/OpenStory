using System;
using System.Net.Sockets;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents an encrypted network session.
    /// </summary>
    public sealed class ServerSession
    {
        private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

        private BoundedBuffer headerBuffer;
        private BoundedBuffer packetBuffer;
        private NetworkSession session;

        /// <summary>
        /// A unique 32-bit session ID.
        /// </summary>
        public int SessionId { get; private set; }

        /// <summary>
        /// The cryptographic transformer for this session.
        /// </summary>
        public ServerCrypto Crypto { get; private set; }

        /// <summary>
        /// The event used to handle incoming packets.
        /// </summary>
        public event EventHandler<IncomingPacketEventArgs> OnPacketReceived;

        /// <summary>
        /// The event raised when the session is closed.
        /// </summary>
        public event EventHandler OnClosing;
        
        /// <summary>
        /// Initializes a new instance of the Session class.
        /// </summary>
        /// <param name="socket">The underlying socket for this session.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket"/> is <c>null</c>.
        /// </exception>
        public ServerSession(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            this.session = new NetworkSession(socket);
            this.session.OnDataArrived += this.HandleIncomingData;
            this.session.OnClosing += this.HandleClosing;

            this.packetBuffer = new BoundedBuffer();
            this.headerBuffer = new BoundedBuffer(4);

            this.SessionId = RollingSessionId.Increment();
        }

        private void HandleClosing(object sender, EventArgs e)
        {
            if (this.OnClosing != null)
            {
                this.OnClosing(this, e);
            }
        }

        /// <summary>
        /// Initiates the session operations.
        /// </summary>
        /// <param name="clientIV">The client IV to use for the cryptographic transformation.</param>
        /// <param name="serverIV">The server IV to use for the cryptographic transformation.</param>
        /// <param name="version">The game version.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the method is called when the 
        /// <see cref="OnPacketReceived"/> 
        /// event has no subscribers.
        /// </exception>
        public void Start(byte[] clientIV, byte[] serverIV, ushort version)
        {
            if (this.OnPacketReceived == null)
            {
                throw new InvalidOperationException("'OnPacketReceived' has no subscribers.");
            }
            this.Crypto = new ServerCrypto(clientIV, serverIV, version);

            byte[] helloPacket = ConstructHelloPacket(clientIV, serverIV, version);
            this.session.Start();
            this.session.Write(helloPacket);
            this.packetBuffer.Reset(0);
        }

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void Close()
        {
            this.OnPacketReceived = null;
            this.session.Close();
        }

        #region Outgoing logic

        /// <summary>
        /// Encrypts the given data as a packet 
        /// and writes it to the network stream.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packet" /> is <c>null</c>.
        /// </exception>
        public void WritePacket(byte[] packet)
        {
            if (packet == null) throw new ArgumentNullException("packet");

            byte[] rawData = this.Crypto.EncryptAndPack(packet);
            if (this.session.Socket.Connected) this.session.Write(rawData);
        }

        private static byte[] ConstructHelloPacket(byte[] clientIV, byte[] serverIV, ushort version)
        {
            using (var builder = new PacketBuilder(16))
            {
                builder.WriteInt16(0x0E);
                builder.WriteInt16(version);
                builder.WriteLengthString("2"); // supposedly some patch thing?
                builder.WriteBytes(clientIV);
                builder.WriteBytes(serverIV);

                // Test server flag.
                builder.WriteByte(0x05);

                return builder.ToByteArray();
            }
        }
        
        #endregion

        #region Incoming logic

        private void HandleIncomingData(object sender, DataArrivedEventArgs args)
        {
            byte[] data = args.Data;
            int position = 0, remaining = data.Length;
            while (this.packetBuffer.FreeSpace == 0)
            {
                byte[] rawData = this.packetBuffer.ExtractAndReset(0);
                if (rawData.Length > 0)
                {
                    this.DecryptAndHandle(rawData);
                }

                if (remaining == 0) break;

                int bufferred;
                int headerRemaining = this.headerBuffer.FreeSpace;
                if (headerRemaining > 0)
                {
                    bufferred = this.headerBuffer.AppendFill(data, position, headerRemaining);

                    // For the confused: if we didn't fill the header, it 
                    // means the data array didn't have enough elements.
                    // We move on.
                    if (bufferred < headerRemaining) break;

                    position += bufferred;
                    remaining -= bufferred;
                }

                byte[] header = this.headerBuffer.ExtractAndReset(4);
                int length = this.Crypto.TryGetLength(header);
                if (length == -1)
                {
                    Log.WriteError("Header {0} invalid, closing connection...", BitConverter.ToString(header));
                    this.Close();
                    return;
                }

                this.packetBuffer.Reset(length);

                bufferred = this.packetBuffer.AppendFill(data, position, remaining);
                position += bufferred;
                remaining -= bufferred;
            }
        }

        private void DecryptAndHandle(byte[] data)
        {
            this.Crypto.Decrypt(data);

            var args = new IncomingPacketEventArgs(data);
            this.OnPacketReceived(this, args);
        }

        #endregion
    }
}