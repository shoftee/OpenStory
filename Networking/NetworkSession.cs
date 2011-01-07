using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using OpenMaple.Cryptography;
using OpenMaple.Threading;
using OpenMaple.Tools;

namespace OpenMaple.Networking
{
    internal delegate void OnDataDelegate(byte[] data);

    sealed class NetworkSession : IDescriptorContainer
    {
        #region Factory

        /// <summary>
        /// An AtomicInteger used for getting valid new session IDs.
        /// </summary>
        private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

        private const int PoolCapacity = 200;

        private static readonly ConcurrentBag<NetworkSession> SessionPool =
            new ConcurrentBag<NetworkSession>();

        /// <summary>
        /// Provides a new session instance for the given socket.
        /// </summary>
        /// <param name="clientSocket">The socket to bind the new session instance to.</param>
        /// <returns>A new session bound to <paramref name="clientSocket"/>.</returns>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="clientSocket"/> is null.</exception>
        public static NetworkSession New(Socket clientSocket)
        {
            if (clientSocket == null) throw new ArgumentNullException("clientSocket");

            NetworkSession networkSession;
            if (!SessionPool.TryTake(out networkSession))
            {
                networkSession = new NetworkSession();
            }
            networkSession.Open(clientSocket);
            return networkSession;
        }

        #endregion

        #region Fields and properties

        private static readonly short MapleVersion = Properties.Settings.Default.MapleVersion;

        /// <summary>
        /// Denotes whether the socket is currently disconnected or not.
        /// The condition for the name is inverted because connected-ness is the default state.
        /// </summary>
        private AtomicBoolean isDisconnected;

        private Socket socket;
        private ReceiveDescriptor receiveDescriptor;
        private SendDescriptor sendDescriptor;

        /// <summary>
        /// A unique ID for the current session.
        /// When the session is not active, this is null.
        /// </summary>
        public int? SessionId { get; private set; }

        /// <summary>
        /// The rolling AES encryption for the output stream.
        /// </summary>
        public AesEncryption SendCrypto { get; private set; }

        /// <summary>
        /// The rolling AES encryption for the input stream.
        /// </summary>
        public AesEncryption ReceiveCrypto { get; private set; }

        /// <summary>
        /// The IPv4 dotted-quad for the remote end-point of this session's socket.
        /// When the session is not active, this is <see cref="String.Empty"/>.
        /// </summary>
        public string RemoteAddress { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new NetworkSession.
        /// </summary>
        private NetworkSession()
        {
            short version = MapleVersion;

            byte[] sendIv = ByteUtils.GetNewIV();
            this.SendCrypto = new AesEncryption(sendIv, (short) (0xFFFF - version));

            byte[] receiveIv = ByteUtils.GetNewIV();
            this.ReceiveCrypto = new AesEncryption(receiveIv, version);

            this.receiveDescriptor = new ReceiveDescriptor(this);
            this.sendDescriptor = new SendDescriptor(this);

            this.SetSocket(null);
        }

        #region Connection methods

        /// <summary>
        /// Activates the NetworkSession with the given parameters.
        /// </summary>
        /// <param name="clientSocket">The socket for the network session.</param>
        private void Open(Socket clientSocket)
        {
            this.SetSocket(clientSocket);

            this.isDisconnected = new AtomicBoolean(false);

            // TODO: BufferPool
            var receiveBuffer = new ArraySegment<byte>();
            this.receiveDescriptor.SetBuffer(receiveBuffer);
            this.receiveDescriptor.OnData += this.ReceiveData;

            this.sendDescriptor.Open();

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

            this.socket.Dispose();
            this.SetSocket(null);

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

        /// <summary>
        /// Assigns a new socket to this NetworkSession instance and updates the SessionId and RemoteAddress string.
        /// If <paramref name="newSocket"/> is null, SessionId is set to -1 and RemoteAddress is set to <see cref="String.Empty"/>.
        /// </summary>
        /// <param name="newSocket">The new socket.</param>
        private void SetSocket(Socket newSocket)
        {
            this.socket = newSocket;
            if (newSocket == null)
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

        private static InvalidOperationException GetSessionNotOpenException()
        {
            return new InvalidOperationException("This session is not open.");
        }

        #endregion

        #region IO methods

        /// <summary>
        /// Writes the given packet to the output stream.
        /// </summary>
        /// <param name="data">The packet data to write.</param>
        /// <exception cref="InvalidOperationException">The exception is thrown if this session is not open.</exception>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="data"/> is null.</exception>
        public void Write(byte[] data)
        {
            if (this.socket == null) throw GetSessionNotOpenException();
            if (data == null) throw new ArgumentNullException("data");

            // This is the only method that modifies SendCrypto.
            Synchronizer.Execute(() => this.WriteWithCrypto(data));
        }

        /// <summary>
        /// Encrypts the given data as a packet and writes it to the network stream.
        /// This method is to be used only within synchronization queues.
        /// </summary>
        /// <param name="data">The data to send.</param>
        private void WriteWithCrypto(byte[] data)
        {
            int length = data.Length;
            byte[] packet = new byte[length + 4];

            byte[] header = this.SendCrypto.ConstructHeader(length);
            Buffer.BlockCopy(header, 0, packet, 0, 4);

            byte[] encrypted = new byte[length];
            Buffer.BlockCopy(data, 0, encrypted, 0, length);
            this.SendCrypto.Transform(encrypted);
            CustomEncryption.Encrypt(encrypted);

            Buffer.BlockCopy(encrypted, 0, packet, 4, length);

            this.sendDescriptor.Send(packet);
        }

        #endregion

        #region IDescriptorContainer explicit implementation

        bool IDescriptorContainer.IsDisconnected { get { return this.isDisconnected.Value; } }
        Socket IDescriptorContainer.Socket { get { return this.socket; } }
        
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

    internal interface IDescriptorContainer
    {
        Socket Socket { get; }
        bool IsDisconnected { get; }

        void Close();
    }
}
