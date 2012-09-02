using System;
using System.Net;
using System.Net.Sockets;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Server.Fluent;

namespace OpenStory.Server
{
    /// <summary>
    /// A base class for services which handle public communication.
    /// </summary>
    public abstract class AbstractServer
    {
        private readonly SocketAcceptor acceptor;
        private readonly RollingIvFactory ivFactory;

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AbstractServer"/>.
        /// </summary>
        /// <param name="address">The address to listen on.</param>
        /// <param name="port">The port to listen on.</param>
        protected AbstractServer(IPAddress address, int port)
        {
            this.IsRunning = false;

            this.acceptor = new SocketAcceptor(address, port);
            this.acceptor.SocketAccepted += (s, e) => this.HandleAccept(e.Socket);

            this.ivFactory = IvFactories.GetEmsFactory();
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.ThrowIfRunning();
            this.IsRunning = true;

            OS.Log().Info("Listening on port {1}.", this.Name, this.acceptor.Port);
            this.acceptor.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            this.ThrowIfNotRunning();

            OS.Log().Info("Shutting down...", this.Name);

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
            byte[] clientIv = GetNewIv();
            byte[] serverIv = GetNewIv();

            var serverSession = new ServerSession();
            serverSession.Closing += OnConnectionClose;

            serverSession.AttachSocket(socket);
            this.OnConnectionOpen(serverSession);

            OS.Log().Info("Session {0} started : CIV {1} SIV {2}.", serverSession.NetworkSessionId,
                          BitConverter.ToString(clientIv), BitConverter.ToString(serverIv));

            serverSession.Start(this.ivFactory, clientIv, serverIv);
        }


        private static void OnConnectionClose(object sender, EventArgs args)
        {
            var serverSession = (ServerSession)sender;

            OS.Log().Info("Connection {0} closed.", serverSession.NetworkSessionId);
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

        private static readonly Random Rng = new Random();

        /// <summary>
        /// Returns a new non-zero 4-byte IV array.
        /// </summary>
        /// <returns>a generated 4-byte IV array.</returns>
        private static byte[] GetNewIv()
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