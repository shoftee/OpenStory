using System;
using System.Net.Sockets;
using OpenStory.AccountService;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Server.Common;
using OpenStory.Server.Data;
using OpenStory.Server.Properties;

namespace OpenStory.Server
{
    /// <summary>
    /// A base class for services which handle public communication.
    /// </summary>
    public abstract class AbstractServer
    {
        private static readonly ushort MapleVersion = Settings.Default.MapleVersion;

        private SocketAcceptor acceptor;

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
        /// Gets the name of the server.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.ThrowIfRunning();
            this.IsRunning = true;

            Log.WriteInfo("Listening on port {1}.", this.Name, this.acceptor.Port);
            this.acceptor.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            this.ThrowIfNotRunning();

            Log.WriteInfo("Shutting down...", this.Name);

            this.acceptor.Stop();
            this.IsRunning = false;
        }

        /// <summary>
        /// This method is called when a new client session has been initialized.
        /// </summary>
        /// <remarks>
        /// An internal reference to the session is not kept, when 
        /// overriding this method be sure to save a reference to it.
        /// </remarks>
        /// <param name="serverSession">The new session to process.</param>
        protected abstract void HandleConnectionOpen(ServerSession serverSession);

        private void HandleAccept(Socket socket)
        {
            byte[] clientIV = ByteHelpers.GetNewIV();
            byte[] serverIV = ByteHelpers.GetNewIV();
            var crypto = new ServerCrypto(clientIV, serverIV, MapleVersion);

            var serverSession = new ServerSession(socket, crypto);
            serverSession.OnClosing += HandleConnectionClose;

            byte[] helloPacket = ConstructHelloPacket(clientIV, serverIV);
            this.HandleConnectionOpen(serverSession);

            Log.WriteInfo("Session {0} started : CIV {1} SIV {2}.", serverSession.SessionId,
                          BitConverter.ToString(clientIV), BitConverter.ToString(serverIV));

            serverSession.Start(helloPacket);
        }

        private static byte[] ConstructHelloPacket(byte[] clientIV, byte[] serverIV)
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

        private static void HandleConnectionClose(object sender, EventArgs args)
        {
            var serverSession = sender as ServerSession;
            if (serverSession == null) return;

            Log.WriteInfo("Connection {0} closed.", serverSession.SessionId);
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
                throw new InvalidOperationException(
                    "The server has not been started. Call the Start method before using it.");
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

        class AccountSession : IAccountSession
        {
            private IAccountService parent;

            public int SessionId { get; private set; }

            public int AccountId { get; private set; }
            public string AccountName { get; private set; }

            public AccountSession(IAccountService parent, int sessionId, Account data)
            {
                this.SessionId = sessionId;
                this.AccountId = data.AccountId;
                this.AccountName = data.UserName;

                this.parent = parent;
            }

            public void Dispose()
            {
                parent.UnregisterSession(this.SessionId);
            }
        }

        /// <summary>
        /// Provides an <see cref="IAccountSession"/> with the specified properties.
        /// </summary>
        /// <param name="parent">The account service handling this session.</param>
        /// <param name="sessionId">The account session ID.</param>
        /// <param name="data">The account data for this session.</param>
        /// <returns>a reference to the constructed <see cref="IAccountSession"/>.</returns>
        protected IAccountSession GetSession(IAccountService parent, int sessionId, Account data)
        {
            return new AccountSession(parent, sessionId, data);
        }
    }
}