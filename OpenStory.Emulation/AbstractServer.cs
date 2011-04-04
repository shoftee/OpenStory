using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Server;

namespace OpenStory.Emulation
{
    /// <summary>
    /// A base class for Server instances.
    /// </summary>
    abstract class AbstractServer
    {
        private SocketAcceptor acceptor;

        protected abstract string ServerName { get; }

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initializes a new instance of AbstractServer and binds the internal acceptor to the given port.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        protected AbstractServer(int port)
        {
            this.IsRunning = false;

            this.acceptor = new SocketAcceptor(port);
            this.acceptor.OnSocketAccepted += (s, e) => this.HandleAccept(e.Socket);
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.ThrowIfRunning();
            this.IsRunning = true;
            this.StartAccepting();
        }

        private void StartAccepting()
        {
            Log.WriteInfo("[{0}] Listening on port {1}.", this.ServerName, acceptor.Port);
            this.acceptor.Start();
        }

        /// <summary>
        /// Shuts down the server after the given delay.
        /// </summary>
        /// <param name="delay">The time to wait before initiating the shut down procedure.</param>
        public void ShutDown(TimeSpan delay)
        {
            this.ThrowIfNotRunning();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if the <see cref="IsRunning"/> property is true and throws an exception if it is not.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the server is not running.
        /// </exception>
        protected void ThrowIfNotRunning()
        {
            if (!this.IsRunning)
            {
                throw new InvalidOperationException("The server has not been started. Call the Start method before using it.");
            }
        }

        /// <summary>
        /// Checks if the <see cref="IsRunning"/> property is true and throws and exception if it is.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the server is running.
        /// </exception>
        protected void ThrowIfRunning()
        {
            if (this.IsRunning)
            {
                throw new InvalidOperationException("The server is already running.");
            }
        }

        /// <summary>
        /// This method is called when a new client session has been initialized.
        /// </summary>
        /// <remarks>
        /// An internal reference to the session is not kept, when 
        /// overriding this method be sure to save a reference to it.
        /// </remarks>
        /// <param name="serverSession">The new session to process.</param>
        protected abstract void HandleSession(ServerSession serverSession);

        private void HandleAccept(Socket socket)
        {
            ServerSession serverSession = new ServerSession(socket, GetUnpacker(), GetPacker());
            serverSession.OnClosing += HandleSessionClose;

            var clientIV = serverSession.Unpacker.IV;
            var serverIV = serverSession.Packer.IV;
            byte[] helloPacket = ConstructHelloPacket(clientIV, serverIV);
            this.HandleSession(serverSession);

            Log.WriteInfo("Session {0} started : CIV {1} SIV {2}.", serverSession.SessionId, BitConverter.ToString(clientIV), BitConverter.ToString(serverIV));

            serverSession.Start(helloPacket);
        }

        private byte[] ConstructHelloPacket(byte[] clientIV, byte[] serverIV)
        {
            using (var builder = new PacketBuilder(16))
            {
                builder.WriteInt16(0x0E);
                builder.WriteInt16(MapleVersion);
                builder.WriteLengthString("2"); // supposedly some patch thing?
                builder.WriteBytes(clientIV);
                builder.WriteBytes(serverIV);

                // Test server flag.
                builder.WriteByte(0x05);

                return builder.ToByteArray();
            }
        }

        #region Static crypto pooling

        private static readonly ushort MapleVersion = Properties.Settings.Default.MapleVersion;

        // Server-side specific, packers use regular version representation.
        private static readonly ConcurrentQueue<Packer> PackerPool =
            new ConcurrentQueue<Packer>();
        // Server-side specific, unpackers use two's complement of the version.
        private static readonly ConcurrentQueue<Unpacker> UnpackerPool =
            new ConcurrentQueue<Unpacker>();

        private const int CryptoPoolingCapacity = 100;

        private static Packer GetPacker()
        {
            Packer crypto;
            if (!PackerPool.TryDequeue(out crypto))
            {
                byte[] iv = ByteHelpers.GetNewIV();
                crypto = new Packer(iv, MapleVersion, VersionType.Complement);
            }
            return crypto;
        }

        private static Unpacker GetUnpacker()
        {
            Unpacker crypto;
            if (!UnpackerPool.TryDequeue(out crypto))
            {
                byte[] iv = ByteHelpers.GetNewIV();
                crypto = new Unpacker(iv, MapleVersion, VersionType.Regular);
            }
            return crypto;
        }

        private static void HandleSessionClose(object sender, EventArgs args)
        {
            ServerSession serverSession = sender as ServerSession;
            if (serverSession == null) return;

            Log.WriteInfo("Session {0} closed.", serverSession.SessionId);

            ReclaimCryptos(serverSession);
        }

        private static void ReclaimCryptos(ServerSession serverSession)
        {
            if (PackerPool.Count < CryptoPoolingCapacity)
            {
                PackerPool.Enqueue(serverSession.Packer);
            }
            if (UnpackerPool.Count < CryptoPoolingCapacity)
            {
                UnpackerPool.Enqueue(serverSession.Unpacker);
            }
        }

        #endregion
    }
}
