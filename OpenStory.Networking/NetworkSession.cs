using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenStory.Common;
using OpenStory.Cryptography;
using OpenStory.Networking.Properties;
using OpenStory.Synchronization;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a network session used for sending and receiving data.
    /// </summary>
    public sealed class NetworkSession : IReceiveDescriptorContainer, ISendDescriptorContainer
    {
        #region Factory

        private const int PoolCapacity = 200;

        /// <summary>
        /// An AtomicInteger used for getting valid new session IDs.
        /// </summary>
        private static readonly AtomicInteger RollingSessionId = new AtomicInteger(0);

        private static readonly ConcurrentBag<NetworkSession> SessionPool =
            new ConcurrentBag<NetworkSession>();

        /// <summary>
        /// Provides a new session instance for the given socket.
        /// </summary>
        /// <param name="socket">The socket to bind the new session instance to.</param>
        /// <returns>A new session bound to <paramref name="socket"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="socket"/> is <c>null</c>.
        /// </exception>
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

        private Socket socket;
        private ReceiveDescriptor receiveDescriptor;
        private SendDescriptor sendDescriptor;

        /// <summary> A unique ID for the current session. When the session is not active, this is <c>null</c>.</summary>
        public int? SessionId { get; private set; }

        /// <summary> The rolling AES encryption for the output stream.</summary>
        public AesEncryption SendCrypto { get; private set; }

        /// <summary>The rolling AES encryption for the input stream.</summary>
        public AesEncryption ReceiveCrypto { get; private set; }

        /// <summary> Denotes whether the socket is currently disconnected or not.</summary>
        /// <remarks> The condition for the name is inverted because connected-ness is common.</remarks>
        public bool IsDisconnected
        {
            get { return this.isDisconnected.Value; }
        }

        /// <summary>
        /// Gets the socket being used for this session. If the session is inactive, this is <c>null</c>.
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
                }
                else
                {
                    this.SessionId = RollingSessionId.Increment();
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new NetworkSession.
        /// </summary>
        private NetworkSession()
        {
            short version = MapleVersion;

            byte[] sendIv = ByteHelpers.GetNewIV();
            this.SendCrypto = new AesEncryption(sendIv, (short) (0xFFFF - version));

            byte[] receiveIv = ByteHelpers.GetNewIV();
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
            Synchronizer.ScheduleAction(this.InitializeConnection);
        }

        private void InitializeConnection()
        {
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

            this.Socket = null;
            this.socket.Dispose();

            this.receiveDescriptor.Close();
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

        #endregion

        /// <summary>
        /// Writes a byte array to the network stream.
        /// </summary>
        /// <param name="data">The data to write.</param>
        public void Write(byte[] data)
        {
            this.sendDescriptor.Write(data);
        }

        #region IDescriptorContainer explicit implementations

        void IReceiveDescriptorContainer.Close()
        {
            this.Close();
        }

        void ISendDescriptorContainer.Close()
        {
            this.Close();
        }

        #endregion
    }
}