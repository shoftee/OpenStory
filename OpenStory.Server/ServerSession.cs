using System;
using System.Net.Sockets;
using OpenStory.Common;
using OpenStory.Common.IO;
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

        /// <summary>
        /// The event used to handle incoming packets.
        /// </summary>
        public event EventHandler<IncomingPacketEventArgs> OnPacketReceived;

        /// <summary>
        /// The event raised when the session is closed.
        /// </summary>
        public event EventHandler OnClosing
        {
            add { session.OnClosing += value; }
            remove { session.OnClosing -= value; }
        }

        /// <summary>
        /// A unique 32-bit session ID.
        /// </summary>
        public int SessionId { get; private set; }

        private NetworkSession session;

        private BoundedBuffer packetBuffer;
        private BoundedBuffer headerBuffer;

        /// <summary>
        /// Gets the <see cref="Packer"/> used to transform outgoing data.
        /// </summary>
        public Packer Packer { get; private set; }

        /// <summary>
        /// Gets the <see cref="Unpacker"/> used to transform incoming data.
        /// </summary>
        public Unpacker Unpacker { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Session class.
        /// </summary>
        /// <param name="socket">The underlying socket for this network session.</param>
        /// <param name="unpacker">An <see cref="Unpacker"/> to use for incoming data.</param>
        /// <param name="packer">A <see cref="Packer"/> to use for outgoing data.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the parameters is <c>null</c>.
        /// </exception>
        public ServerSession(Socket socket, Unpacker unpacker, Packer packer)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            if (packer == null) throw new ArgumentNullException("packer");
            if (unpacker == null) throw new ArgumentNullException("unpacker");

            this.session = new NetworkSession(socket);
            this.session.OnDataArrived += this.HandleIncomingData;

            this.Packer = packer;
            this.Unpacker = unpacker;

            this.packetBuffer = new BoundedBuffer();
            this.headerBuffer = new BoundedBuffer(4);

            this.SessionId = RollingSessionId.Increment();
        }

        /// <summary>
        /// Initiates the session operations.
        /// </summary>
        /// <param name="helloPacket">The hello packet to send for the handshake.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="helloPacket"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the method is called when the 
        /// <see cref="OnPacketReceived"/> received 
        /// event has no subscribers.
        /// </exception>
        public void Start(byte[] helloPacket)
        {
            if (helloPacket == null) throw new ArgumentNullException("helloPacket");
            if (this.OnPacketReceived == null)
            {
                throw new InvalidOperationException("'OnPacketReceived' has no subscribers.");
            }

            session.Start();
            session.Write(helloPacket);
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

            byte[] rawData = this.Packer.EncryptAndPack(packet);

            session.Write(rawData);
        }

        #endregion

        #region Incoming logic

        private void HandleIncomingData(object sender, DataArrivedEventArgs args)
        {
            byte[] data = args.Data;
            int position = 0, remaining = data.Length;
            while (packetBuffer.FreeSpace == 0)
            {
                int headerRemaining = this.headerBuffer.FreeSpace;
                int bufferred;
                if (headerRemaining > 0)
                {
                    bufferred = this.headerBuffer.AppendFill(data, headerRemaining);

                    // For the confused: if we didn't fill the header, it 
                    // means the data array had less elements than we need.
                    // We move on.
                    if (bufferred < headerRemaining) break;

                    position += bufferred;
                    remaining -= bufferred;
                }

                byte[] header = this.headerBuffer.ExtractAndReset(4);
                if (this.Unpacker.CheckHeader(header))
                {
                    this.Close();
                }

                int length = AesTransform.GetPacketLength(header);
                byte[] rawData = packetBuffer.ExtractAndReset(length);
                if (rawData.Length > 0)
                {
                    this.DecryptAndHandle(rawData);
                }

                bufferred = packetBuffer.AppendFill(data, position, remaining);
                position += bufferred;
                remaining -= bufferred;
            }
        }

        private void DecryptAndHandle(byte[] data)
        {
            this.Unpacker.Decrypt(data);

            var args = new IncomingPacketEventArgs(data);
            this.OnPacketReceived(this, args);
        }

        #endregion

        /// <summary>
        /// Closes the session.
        /// </summary>
        public void Close()
        {
            this.OnPacketReceived = null;
            session.Close();
        }
    }
}
