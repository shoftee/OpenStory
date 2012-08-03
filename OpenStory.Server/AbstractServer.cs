using System;
using System.Net.Sockets;
using OpenStory.Common.Tools;
using OpenStory.Networking;
using OpenStory.Server.Data;
using OpenStory.Server.Properties;
using OpenStory.ServiceModel;

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
            this.acceptor.SocketAccepted += (s, e) => this.HandleAccept(e.Socket);
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
        protected abstract void OnConnectionOpen(ServerSession serverSession);

        private void HandleAccept(Socket socket)
        {
            byte[] clientIV = GetNewIV();
            byte[] serverIV = GetNewIV();

            var serverSession = new ServerSession();
            serverSession.Closing += OnConnectionClose;

            serverSession.AttachSocket(socket);
            this.OnConnectionOpen(serverSession);

            Log.WriteInfo("Session {0} started : CIV {1} SIV {2}.", serverSession.NetworkSessionId,
                          BitConverter.ToString(clientIV), BitConverter.ToString(serverIV));

            serverSession.Start(clientIV, serverIV, MapleVersion);
        }


        private static void OnConnectionClose(object sender, EventArgs args)
        {
            var serverSession = (ServerSession) sender;

            Log.WriteInfo("Connection {0} closed.", serverSession.NetworkSessionId);
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
            private readonly IAccountService parent;

            /// <inheritdoc />
            public int SessionId { get; private set; }

            /// <inheritdoc />
            public int AccountId { get; private set; }

            /// <inheritdoc />
            public string AccountName { get; private set; }

            /// <summary>
            /// Initializes a new instance of AccountSession.
            /// </summary>
            /// <param name="parent">The <see cref="IAccountService"/> managing this session.</param>
            /// <param name="sessionId">The session identifier.</param>
            /// <param name="data">The loaded session data.</param>
            public AccountSession(IAccountService parent, int sessionId, Account data)
            {
                this.SessionId = sessionId;
                this.AccountId = data.AccountId;
                this.AccountName = data.UserName;

                this.parent = parent;
            }

            /// <inheritdoc />
            public void Dispose()
            {
                parent.TryUnregisterSession(this.AccountId);
            }
        }

        /// <summary>
        /// Provides an <see cref="OpenStory.ServiceModel.IAccountSession"/> with the specified properties.
        /// </summary>
        /// <param name="parent">The account service handling this session.</param>
        /// <param name="sessionId">The account session ID.</param>
        /// <param name="data">The account data for this session.</param>
        /// <returns>a reference to the constructed <see cref="OpenStory.ServiceModel.IAccountSession"/>.</returns>
        protected IAccountSession GetSession(IAccountService parent, int sessionId, Account data)
        {
            return new AccountSession(parent, sessionId, data);
        }

        private static readonly Random Rng = new Random();

        /// <summary>
        /// Returns a new non-zero 4-byte IV array.
        /// </summary>
        /// <returns>a generated 4-byte IV array.</returns>
        private static byte[] GetNewIV()
        {
            // Just in case we hit that 1 in 2147483648 chance.
            // Things go very bad if the IV is 0.
            int number;
            do number = Rng.Next();
            while (number == 0);

            return BitConverter.GetBytes(number);
        }
    }
}