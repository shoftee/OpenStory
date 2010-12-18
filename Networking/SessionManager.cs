using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using OpenMaple.Cryptography;
using OpenMaple.Tools;

namespace OpenMaple.Networking
{
    class SessionManager
    {
        class Session : ISession
        {
           /// <summary>
            /// The rolling AES encryption for the output stream.
            /// </summary>
            public AesEncryption SendCrypto { get; private set; }

            /// <summary>
            /// The rolling AES encryption for the input stream.
            /// </summary>
            public AesEncryption ReceiveCrypto { get; private set; }

            public Socket Socket { private get; set; }

            /// <summary>
            /// Initializes a new Session with the given socket.
            /// </summary>
            /// <param name="clientSocket">The socket to bind this session to.</param>
            /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="clientSocket"/> is null.</exception>
            public Session(Socket clientSocket)
            {
                if (clientSocket == null) throw new ArgumentNullException("clientSocket");

                this.Socket = clientSocket;

                short version = Properties.Settings.Default.MapleVersion;

                byte[] sendIv = ByteUtils.GetFreshIv();
                this.SendCrypto = new AesEncryption(sendIv, (short) (0xFFFF - version));

                byte[] receiveIv = ByteUtils.GetFreshIv();
                this.ReceiveCrypto = new AesEncryption(receiveIv, version);
            }

            /// <summary>
            /// Writes the given packet to the output stream.
            /// </summary>
            /// <param name="packet">The packet to write.</param>
            /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="packet"/> is null.</exception>
            public void Write(IPacket packet)
            {
                if (packet == null) throw new ArgumentNullException("packet");
                // TODO: Later when I start doing the packet crap~
            }

            /// <summary>
            /// Releases the session so it can be reused with a new socket.
            /// </summary>
            public void Release()
            {
                this.Socket.Dispose();
                this.Socket = null;
                Instance.pool.Add(this);
            }
        }

        private readonly ConcurrentBag<Session> pool;

        private static readonly SessionManager Instance = new SessionManager();
        private SessionManager()
        {
            this.pool = new ConcurrentBag<Session>();
        }

        /// <summary>
        /// Provides a new session instance for the given socket.
        /// </summary>
        /// <param name="socket">The socket to bind the new session instance to.</param>
        /// <returns>A new session bound to <paramref name="socket"/>.</returns>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="socket"/> is null.</exception>
        public static ISession GetNewSession(Socket socket)
        {
            if (socket == null) throw new ArgumentNullException("socket");
            Session session;
            if (Instance.pool.IsEmpty || !Instance.pool.TryTake(out session))
            {
                session = new Session(socket);
            }
            else
            {
                session.Socket = socket;
            }
            return session;
        }
    }

    interface ISession
    {
        AesEncryption SendCrypto { get; }
        AesEncryption ReceiveCrypto { get; }

        void Write(IPacket packet);
        void Release();
    }
}