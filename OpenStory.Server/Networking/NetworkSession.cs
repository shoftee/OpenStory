using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using OpenStory.Common.Threading;
using OpenStory.Cryptography;
using OpenStory.Server.Properties;
using OpenStory.Server.Synchronization;

namespace OpenStory.Server.Networking
{
    public sealed class NetworkSession : IDescriptorContainer
    {
        #region Factory

        private const int PoolCapacity = 200;

        /// <summary>An AtomicInteger used for getting valid new session IDs.</summary>
        private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

        private static readonly ConcurrentBag<NetworkSession> SessionPool =
            new ConcurrentBag<NetworkSession>();

        /// <summary>Provides a new session instance for the given socket.</summary>
        /// <param name="socket">The socket to bind the new session instance to.</param>
        /// <returns>A new session bound to <paramref name="socket"/>.</returns>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="socket"/> is null.</exception>
        public static NetworkSession New(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");

            NetworkSession networkSession;
            if (!SessionPool.TryTake(out networkSession))
            {
                networkSession = new NetworkSession();
            }
            networkSession.Open(socket);
            return networkSession;
        }

        #endregion

        #region Fields and properties

        private static readonly short MapleVersion = Settings.Default.MapleVersion;

        private AtomicBoolean isDisconnected;

        private ReceiveDescriptor receiveDescriptor;
        private SendDescriptor sendDescriptor;
        private Socket socket;

        /// <summary> A unique ID for the current session. When the session is not active, this is null.</summary>
        public int? SessionId { get; private set; }

        /// <summary> The rolling AES encryption for the output stream.</summary>
        public AesEncryption SendCrypto { get; private set; }

        /// <summary>The rolling AES encryption for the input stream.</summary>
        public AesEncryption ReceiveCrypto { get; private set; }

        /// <summary>
        /// The IPv4 dotted-quad for the remote end-point of this session's socket.
        /// When the session is not active, this is <see cref="String.Empty"/>.
        /// </summary>
        public string RemoteAddress { get; private set; }

        /// <summary> Denotes whether the socket is currently disconnected or not.</summary>
        /// <remarks> The condition for the name is inverted because connected-ness is common.</remarks>
        public bool IsDisconnected
        {
            get { return this.isDisconnected.Value; }
        }

        /// <summary>
        /// Gets the socket being used for this session. If the session is inactive, this is null.
        /// </summary>
        public Socket Socket
        {
            get { return this.socket; }
            private set
            {
                this.socket = value;
                if (value == null)
                {
                    this.SessionId = null;
                    this.RemoteAddress = String.Empty;
                }
                else
                {
                    this.SessionId = RollingSessionId.Increment();
                    this.RemoteAddress = ((IPEndPoint) this.socket.RemoteEndPoint).Address.ToString();
                }
            }
        }

        #endregion

        /// <summary>Initializes a new NetworkSession.</summary>
        private NetworkSession()
        {
            short version = MapleVersion;

            byte[] sendIv = ByteUtils.GetNewIV();
            this.SendCrypto = new AesEncryption(sendIv, (short) (0xFFFF - version));

            byte[] receiveIv = ByteUtils.GetNewIV();
            this.ReceiveCrypto = new AesEncryption(receiveIv, version);

            this.receiveDescriptor = new ReceiveDescriptor(this);
            this.sendDescriptor = new SendDescriptor(this);

            this.Socket = null;
        }

        #region Connection methods

        /// <summary>Activates the NetworkSession with the given parameters.</summary>
        /// <param name="clientSocket">The socket for the network session.</param>
        private void Open(Socket clientSocket)
        {
            this.Socket = clientSocket;

            this.isDisconnected = new AtomicBoolean(false);

            // TODO: BufferPool
            var receiveBuffer = new ArraySegment<byte>();
            this.receiveDescriptor.SetBuffer(receiveBuffer);
            this.receiveDescriptor.OnData += this.ReceiveData;

            Synchronizer.ScheduleAction(this.InitializeConnection);
        }

        private void InitializeConnection()
        {
            this.sendDescriptor.Open();
            // TODO: Send hello packet
            this.receiveDescriptor.StartReceive();
        }

        /// <summary>
        /// Releases the session so it can be reused with a new socket.
        /// </summary>
        public void Close()
        {
            if (this.isDisconnected.CompareExchange(false, true))
            {
                return;
            }

            this.Socket = null;
            this.socket.Dispose();

            this.receiveDescriptor.Close();
            this.receiveDescriptor.OnData -= this.ReceiveData;

            this.sendDescriptor.Close();

            this.Release();
        }

        private void Release()
        {
            if (SessionPool.Count < PoolCapacity)
            {
                SessionPool.Add(this);
            }
        }

        private static InvalidOperationException GetSessionNotOpenException()
        {
            return new InvalidOperationException("This session is not open.");
        }

        #endregion

        #region IO methods

        /// <summary>Encrypts a packet, adds a header to it, and writes it to the output stream.</summary>
        /// <param name="data">The packet data to write.</param>
        /// <exception cref="InvalidOperationException">The exception is thrown if this session is not open.</exception>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="data"/> is null.</exception>
        public void Write(byte[] data)
        {
            if (this.socket == null) throw GetSessionNotOpenException();
            if (data == null) throw new ArgumentNullException("data");

            // This is the only method that modifies SendCrypto.
            Synchronizer.ScheduleAction(() => this.EncryptAndWrite(data));
        }

        /// <summary>
        /// Encrypts the given data as a packet and writes it to the network stream.
        /// This method is to be used only within synchronization queues.
        /// </summary>
        /// <param name="packet">The data to send.</param>
        private void EncryptAndWrite(byte[] packet)
        {
            int length = packet.Length;
            var rawData = new byte[length + 4];

            byte[] header = this.SendCrypto.ConstructHeader(length);
            Buffer.BlockCopy(header, 0, rawData, 0, 4);

            var encrypted = new byte[length];
            Buffer.BlockCopy(packet, 0, encrypted, 0, length);
            this.SendCrypto.Transform(encrypted);
            CustomEncryption.Encrypt(encrypted);

            Buffer.BlockCopy(encrypted, 0, rawData, 4, length);

            this.sendDescriptor.Send(rawData);
        }

        #endregion

        #region IDescriptorContainer explicit implementation

        void IDescriptorContainer.Close()
        {
            this.Close();
        }

        #endregion

        private void ReceiveData(byte[] receivedBlock)
        {
            // TODO: data concatenation...
        }
    }
}