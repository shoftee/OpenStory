using System;
using System.Timers;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Networking;
using OpenStory.Services.Contracts;
using Ninject.Extensions.Logging;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// </summary>
    /// <remarks>
    /// This class is abstract.
    /// </remarks>
    public abstract class ClientBase : IDisposable
    {
        /// <summary>
        /// The number of pings a client is allowed to miss before being disconnected.
        /// </summary>
        private const int MissedPingsAllowed = 3;

        /// <summary>
        /// The period between pings, in milliseconds.
        /// </summary>
        private const int PingInterval = 30000;

        private readonly Timer _keepAliveTimer;
        private readonly AtomicInteger _sentPings;

        private IAccountSession _accountSession;

        private bool _isDisposed;

        /// <summary>
        /// Occurs when the client's session is being closed.
        /// </summary>
        public event EventHandler<ConnectionClosingEventArgs> Closing;

        /// <summary>
        /// Gets the client's session object.
        /// </summary>
        protected IServerSession ServerSession { get; }

        /// <summary>
        /// Gets or sets the account session object.
        /// </summary>
        /// <remarks>
        /// This object is null if the client has not logged in.
        /// </remarks>
        protected IAccountSession AccountSession
        {
            get { return _accountSession; }
            set { _accountSession = value; }
        }

        /// <summary>
        /// Gets or sets the account for the current session.
        /// </summary>
        /// <remarks>
        /// This object is null if the client has not logged in.
        /// </remarks>
        protected Account Account { get; set; }

        /// <summary>
        /// Gets the packet factory that the client uses to create new packets.
        /// </summary>
        protected IPacketFactory PacketFactory { get; }

        /// <summary>
        /// Gets the logger that the client uses to log events.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase" /> class.
        /// </summary>
        /// <param name="serverSession">The network session to bind the instance to.</param>
        /// <param name="packetFactory">The <see cref="IPacketFactory" /> to use for this client.</param>
        /// <param name="logger">The logger to use for this client.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters is <see langword="null" />.</exception>
        protected ClientBase(IServerSession serverSession, IPacketFactory packetFactory, ILogger logger)
        {
            if (serverSession == null)
            {
                throw new ArgumentNullException(nameof(serverSession));
            }
            if (packetFactory == null)
            {
                throw new ArgumentNullException(nameof(packetFactory));
            }

            _isDisposed = false;
            _sentPings = new AtomicInteger(0);

            ServerSession = InitializeSession(serverSession);
            PacketFactory = packetFactory;
            Logger = logger;

            _keepAliveTimer = InitializeTimer();
            _keepAliveTimer.Start();
        }

        private IServerSession InitializeSession(IServerSession serverSession)
        {
            serverSession.PacketProcessing += OnPacketProcessing;
            serverSession.Closing += OnSessionClosing;
            return serverSession;
        }

        private Timer InitializeTimer()
        {
            var timer = new Timer(PingInterval);
            timer.Elapsed += SendPing;
            return timer;
        }

        private void OnSessionClosing(object sender, ConnectionClosingEventArgs e)
        {
            OnClosing();

            Closing?.Invoke(this, e);
        }

        private void OnClosing()
        {
            _keepAliveTimer.Close();
        }

        #region Packet handling

        private void SendPing(object sender, ElapsedEventArgs e)
        {
            if (_sentPings.Increment() > MissedPingsAllowed)
            {
                Disconnect("No ping response.");
                return;
            }

            using (var ping = PacketFactory.CreatePacket("Ping"))
            {
                ServerSession.WritePacket(ping.ToByteArray());
            }
        }

        private void OnPacketProcessing(object sender, PacketProcessingEventArgs e)
        {
            if (e.Label == "Pong")
            {
                HandlePong();
            }
            else if (e.Label != null)
            {
                Logger.Debug("Received packet '{0}'", e.Label);
                HandlePacket(e);
            }
            else
            {
                var packetCode = e.PacketCode;
                var packetData = e.Reader.ReadFully().ToHex(true);
                Logger.Debug(@"Unrecognized packet code: 0x{0:X4}. Packet buffer: {1}", packetCode, packetData);
            }
        }

        private void HandlePong()
        {
            var session = AccountSession;
            if (session != null)
            {
                TimeSpan lag;
                if (!session.TryKeepAlive(out lag))
                {
                    Disconnect("Session keep-alive failed.");
                    return;
                }
            }

            _sentPings.ExchangeWith(0);
        }

        private void HandlePacket(PacketProcessingEventArgs e)
        {
            try
            {
                ProcessPacket(e);
            }
            catch (IllegalPacketException)
            {
                // TODO: Use IllegalPacketException for penalizing naughty clients.
                Logger.Info("Received illegal packet. Client disconnected.");
                Disconnect("Illegal packet.");
            }
            catch (PacketReadingException)
            {
                Logger.Info("Received incomplete packet. Client disconnected.");
                Disconnect("Incomplete packet.");
            }
        }

        /// <summary>
        /// When implemented in a derived class, processes the provided packet data.
        /// </summary>
        /// <param name="args">The packet to be processed.</param>
        protected abstract void ProcessPacket(PacketProcessingEventArgs args);

        #endregion

        /// <summary>
        /// Writes a packet to the client's stream.
        /// </summary>
        /// <param name="data">The data of the packet.</param>
        public void WritePacket(byte[] data)
        {
            ServerSession.WritePacket(data);
        }

        /// <summary>
        /// Immediately disconnects the client from the server.
        /// </summary>
        /// <param name="reason">The reason for the disconnection.</param>
        public void Disconnect(string reason = null)
        {
            var reasonString = string.IsNullOrWhiteSpace(reason) ? "(no reason supplied)" : reason;

            LogDisconnectReason(AccountSession, reasonString);
            ServerSession.Close(reasonString);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!_isDisposed)
            {
                Misc.AssignNullAndDispose(ref _accountSession);

                _keepAliveTimer.Close();

                ServerSession.Close("Client disposed.");

                _isDisposed = true;
            }
        }

        private void LogDisconnectReason(IAccountSession session, string reason)
        {
            if (session != null)
            {
                Logger.Debug("Account session #{0} was closed: {1}", session.SessionId, reason);
            }
            else
            {
                Logger.Debug("Session was closed: {0}", reason);
            }
        }
    }
}
