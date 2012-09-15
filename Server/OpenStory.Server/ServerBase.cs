using System;
using System.Net;
using System.Net.Sockets;
using OpenStory.Common.Data;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;
using OpenStory.Server.Fluent;
using OpenStory.Server.Processing;
using OpenStory.Server.Properties;

namespace OpenStory.Server
{
    /// <summary>
    /// A base class for services which handle public communication.
    /// </summary>
    public abstract class ServerBase : IGameServer
    {
        private readonly SocketAcceptor acceptor;
        private readonly RollingIvFactory ivFactory;

        /// <summary>
        /// Gets the known op-code table for this server.
        /// </summary>
        protected abstract IOpCodeTable OpCodes { get; }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerBase"/>.
        /// </summary>
        /// <param name="configuration">The server configuration.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="configuration"/> is <c>null</c>.
        /// </exception>
        protected ServerBase(ServerConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            this.IsRunning = false;

            this.ivFactory = IvFactories.GetEmsFactory(Settings.Default.MapleVersion);

            this.acceptor = new SocketAcceptor(configuration.Address, configuration.Port);
            this.acceptor.SocketAccepted += (s, e) => this.HandleAccept(e.Socket);
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.ThrowIfRunning();
            this.IsRunning = true;

            OS.Log().Info("[{0}] Listening on port {1}.", this.Name, this.acceptor.Port);
            this.acceptor.Start();
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            this.ThrowIfNotRunning();

            OS.Log().Info("[{0}] Shutting down...", this.Name);

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
        protected abstract void OnConnectionOpen(IServerSession serverSession);

        /// <inheritdoc />
        public PacketBuilder NewPacket(string label)
        {
            ushort opCode;
            if (!this.OpCodes.TryGetOutgoingOpCode(label, out opCode))
            {
                throw new ArgumentException("The provided label does not correspond to a known packet.", "label");
            }

            var builder = new PacketBuilder();
            builder.WriteInt16(opCode);
            return builder;
        }

        private void HandleAccept(Socket socket)
        {
            byte[] clientIv = GetNewIv();
            byte[] serverIv = GetNewIv();

            var session = this.GetServerSession(socket);
            this.OnConnectionOpen(session);

            OS.Log().Info("Network session {0} started : CIV {1} SIV {2}.",
                          session.NetworkSessionId,
                          clientIv.ToHex(hyphenate: true),
                          serverIv.ToHex(hyphenate: true));

            var info = new ConfiguredHandshakeInfo(Settings.Default.MapleVersion, "2", clientIv, serverIv);
            session.Start(this.ivFactory, info);
        }

        private IServerSession GetServerSession(Socket socket)
        {
            var session = new ServerNetworkSession();
            var wrapper = new ServerGameSession(session, GetLabel);

            wrapper.Closing += OnConnectionClose;
            wrapper.ReadyForPush += HandleReadyForPush;

            session.AttachSocket(socket);

            return wrapper;
        }

        private void HandleReadyForPush(object sender, EventArgs e)
        {
            var wrapper = (ServerGameSession)sender;
            // TODO: do on another thread.
            wrapper.Push();
        }

        private string GetLabel(ushort opCode)
        {
            string label;
            this.OpCodes.TryGetIncomingLabel(opCode, out label);
            return label;
        }

        private static void OnConnectionClose(object sender, EventArgs args)
        {
            var serverSession = (ServerNetworkSession)sender;

            OS.Log().Info("Network session {0} closed.", serverSession.NetworkSessionId);
        }

        #region Exception methods

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
                const string Message =
                    "The server has not been started. Call the Start method before using it.";

                throw new InvalidOperationException(Message);
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
                const string Message = "The server is already running.";

                throw new InvalidOperationException(Message);
            }
        }

        #endregion

        #region IV generation

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

        #endregion
    }
}