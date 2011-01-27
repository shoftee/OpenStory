using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Networking.Properties;
using OpenStory.Synchronization;
using Session = OpenStory.Networking.EncryptedNetworkSession;

namespace OpenStory.Emulation
{
    /// <summary>
    /// A base class for Server instances.
    /// </summary>
    abstract class AbstractServer
    {
        private SocketAcceptor acceptor;

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
            this.acceptor = new SocketAcceptor(port);
            this.acceptor.OnSocketAccepted += (s, e) => this.HandleAccept(e.Socket);
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
            this.StartAccepting();
        }

        private void StartAccepting()
        {
            this.acceptor.Start();
        }

        /// <summary>
        /// Shuts down the server after the given delay.
        /// </summary>
        /// <param name="delay">The time to wait before initiating the shut down procedure.</param>
        public void ShutDown(TimeSpan delay)
        {
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
            if (!IsRunning)
            {
                throw new InvalidOperationException("The server has not been started. Call the Start method before using it.");
            }
        }

        /// <summary>
        /// This method is called when a new client session has been initialized.
        /// </summary>
        /// <remarks>
        /// An internal reference to the session is not kept, when 
        /// overriding this method be sure to save a reference to it.
        /// </remarks>
        /// <param name="session">The new session to process.</param>
        protected abstract void HandleSession(Session session);

        private void HandleAccept(Socket socket)
        {
            Session session = new Session(socket, GetPacker(), GetUnpacker());
            session.OnClose += ReclaimCryptos;
            this.HandleSession(session);
        }

        #region Static crypto pooling

        private static readonly short MapleVersion = Settings.Default.MapleVersion;

        private static readonly ConcurrentQueue<Packer> PackerPool =
            new ConcurrentQueue<Packer>();
        private static readonly ConcurrentQueue<Unpacker> UnpackerPool =
            new ConcurrentQueue<Unpacker>();

        private const int CryptoPoolingCapacity = 100;

        private static Packer GetPacker()
        {
            Packer crypto;
            if (!PackerPool.TryDequeue(out crypto))
            {
                byte[] iv = ByteHelpers.GetNewIV();
                crypto = new Packer(iv, MapleVersion);
            }
            return crypto;
        }

        private static Unpacker GetUnpacker()
        {
            Unpacker crypto;
            if (!UnpackerPool.TryDequeue(out crypto))
            {
                byte[] iv = ByteHelpers.GetNewIV();
                crypto = new Unpacker(iv, MapleVersion);
            }
            return crypto;
        }

        private static void ReclaimCryptos(object sender, EventArgs args)
        {
            Session session = sender as Session;
            if (session == null) return;

            if (PackerPool.Count < CryptoPoolingCapacity)
            {
                PackerPool.Enqueue(session.Packer);
            }
            if (UnpackerPool.Count < CryptoPoolingCapacity)
            {
                UnpackerPool.Enqueue(session.Unpacker);
            }
        }

        #endregion
    }
}
