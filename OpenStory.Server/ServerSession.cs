using System;
using System.Net.Sockets;
using System.Timers;
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

        /// <summary>
        /// The event used to handle incoming packets.
        /// </summary>
        public event EventHandler<IncomingPacketEventArgs> OnPacketReceived;

        /// <summary>
        /// The event raised when the session is closed.
        /// </summary>
        public event EventHandler OnClosing;

        /// <summary>
        /// A unique 32-bit session ID.
        /// </summary>
        public int SessionId { get; private set; }

        private Timer keepAliveTimer;
        private AtomicBoolean receivedPong;
        private static readonly byte[] PingPacket = new byte[] { 0x0F, 0x00 };

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
            this.session.OnClosing += this.HandleClosing;

            this.Packer = packer;
            this.Unpacker = unpacker;

            this.packetBuffer = new BoundedBuffer();
            this.headerBuffer = new BoundedBuffer(4);

            this.keepAliveTimer = new Timer(5000);
            this.keepAliveTimer.Elapsed += this.HandlePing;

            this.receivedPong = new AtomicBoolean(true);

            this.SessionId = RollingSessionId.Increment();
        }

        private void HandlePing(object sender, ElapsedEventArgs e)
        {
            if (!receivedPong.Exchange(false))
            {
                this.session.Close();
                return;
            }
            this.WritePacket(PingPacket);
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
        /// <param name="helloPacket">The hello packet to send for the handshake.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="helloPacket"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the method is called when the 
        /// <see cref="OnPacketReceived"/> 
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
            packetBuffer.Reset(0);
            this.keepAliveTimer.Start();
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
                int length = this.Unpacker.CheckHeaderAndGetLength(header);
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
            this.keepAliveTimer.Dispose();
            this.OnPacketReceived = null;
            session.Close();
        }
    }
}
