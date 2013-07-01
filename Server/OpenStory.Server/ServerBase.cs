using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Framework.Contracts;
using OpenStory.Networking;
using OpenStory.Server.Fluent;
using OpenStory.Server.Processing;
using OpenStory.Server.Properties;
using CommonExceptions = OpenStory.Framework.Model.Common.Exceptions;

namespace OpenStory.Server
{
    /// <summary>
    /// A base class for services which handle public communication.
    /// </summary>
    [Localizable(true)]
    public abstract class ServerBase : IGameServer, IDisposable
    {
        private bool isDisposed;

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

            this.ivFactory = IvFactories.GetEmsFactory(configuration.Version);

            this.acceptor = new SocketAcceptor(configuration.Endpoint);
            this.acceptor.SocketAccepted += (s, e) => this.HandleAccept(e.Socket);
            
            this.IsRunning = false;
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.ThrowIfRunning();
            this.IsRunning = true;

            OS.Log().Info("[{0}] Listening on port {1}.", this.Name, this.acceptor.Endpoint.Port);
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
                throw new ArgumentException(CommonExceptions.UnknownPacketLabel, "label");
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

            // TODO: Load constants from configuration thingie?
            var info = new ConfiguredHandshakeInfo(0x000E, Settings.Default.MapleVersion, "2", clientIv, serverIv, 0x05);
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

            ThreadPool.QueueUserWorkItem(state => wrapper.Push());
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
                throw new InvalidOperationException(CommonExceptions.ServerNotRunning);
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
                throw new InvalidOperationException(CommonExceptions.ServerAlreadyRunning);
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(@"acceptor");
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

        #region Implementation of IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called when the object is being disposed.
        /// </summary>
        /// <remarks>
        /// When overriding this method, call the base implementation before your logic.
        /// </remarks>
        /// <param name="disposing">Whether the object is disposed during a dispose operation or during finalization.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.isDisposed)
            {
                if (this.acceptor != null)
                {
                    this.acceptor.Dispose();
                }

                this.isDisposed = true;
            }
        }

        #endregion
    }
}