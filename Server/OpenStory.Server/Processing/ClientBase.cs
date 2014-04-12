using System;
using System.Timers;
using Ninject.Extensions.Logging;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Framework.Contracts;
using OpenStory.Networking;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Processing
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// This class is abstract.
    /// </summary>
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

        private readonly Timer keepAliveTimer;
        private readonly AtomicInteger sentPings;

        private IAccountSession accountSession;

        private bool isDisposed;

        /// <summary>
        /// Occurs when the client's session is being closed.
        /// </summary>
        public event EventHandler<ConnectionClosingEventArgs> Closing;

        /// <summary>
        /// Gets the client's session object.
        /// </summary>
        protected IServerSession ServerSession { get; private set; }

        /// <summary>
        /// Gets or sets the account session object.
        /// </summary>
        /// <remarks>
        /// This object is null if the client has not logged in.
        /// </remarks>
        protected IAccountSession AccountSession
        {
            get { return this.accountSession; }
            set { this.accountSession = value; }
        }

        /// <summary>
        /// Gets the packet factory that the client uses to create new packets.
        /// </summary>
        protected IPacketFactory PacketFactory { get; private set; }

        /// <summary>
        /// Gets the logger that the client uses to log events.
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBase" /> class.
        /// </summary>
        /// <param name="serverSession">The network session to bind the instance to.</param>
        /// <param name="packetFactory">The <see cref="IPacketFactory" /> to use for this client.</param>
        /// <param name="logger">The logger to use for this client.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the parameters is <see langword="null" />.</exception>
        protected ClientBase(IServerSession serverSession, IPacketFactory packetFactory, ILogger logger)
        {
            Guard.NotNull(() => serverSession, serverSession);
            Guard.NotNull(() => packetFactory, packetFactory);

            this.isDisposed = false;
            this.sentPings = new AtomicInteger(0);

            this.ServerSession = this.InitializeSession(serverSession);
            this.PacketFactory = packetFactory;
            this.Logger = logger;

            this.keepAliveTimer = this.InitializeTimer();
            this.keepAliveTimer.Start();
        }

        private IServerSession InitializeSession(IServerSession serverSession)
        {
            serverSession.PacketProcessing += this.OnPacketProcessing;
            serverSession.Closing += this.OnSessionClosing;
            return serverSession;
        }

        private Timer InitializeTimer()
        {
            var timer = new Timer(PingInterval);
            timer.Elapsed += this.SendPing;
            return timer;
        }

        private void OnSessionClosing(object sender, ConnectionClosingEventArgs e)
        {
            this.OnClosing();

            var handler = this.Closing;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnClosing()
        {
            this.keepAliveTimer.Close();
        }

        #region Packet handling

        private void SendPing(object sender, ElapsedEventArgs e)
        {
            if (this.sentPings.Increment() > MissedPingsAllowed)
            {
                this.Disconnect("No ping response.");
                return;
            }

            using (var ping = this.PacketFactory.CreatePacket("Ping"))
            {
                this.ServerSession.WritePacket(ping.ToByteArray());
            }
        }

        private void OnPacketProcessing(object sender, PacketProcessingEventArgs e)
        {
            if (e.Label == "Pong")
            {
                this.HandlePong();
            }
            else if (e.Label != null)
            {
                this.Logger.Debug("Received packet '{0}'", e.Label);
                this.HandlePacket(e);
            }
            else
            {
                var packetCode = e.PacketCode;
                var packetData = e.Reader.ReadFully().ToHex(true);
                this.Logger.Debug(@"Unrecognized packet code: 0x{0:X4}. Packet buffer: {1}", packetCode, packetData);
            }
        }

        private void HandlePong()
        {
            var session = this.AccountSession;
            if (session != null)
            {
                TimeSpan lag;
                if (!session.TryKeepAlive(out lag))
                {
                    this.Disconnect("Session keep-alive failed.");
                    return;
                }
            }

            this.sentPings.ExchangeWith(0);
        }

        private void HandlePacket(PacketProcessingEventArgs e)
        {
            try
            {
                this.ProcessPacket(e);
            }
            catch (IllegalPacketException)
            {
                // TODO: Use IllegalPacketException for penalizing naughty clients.
                this.Logger.Info("Received illegal packet. Client disconnected.");
                this.Disconnect("Illegal packet.");
            }
            catch (PacketReadingException)
            {
                this.Logger.Info("Received incomplete packet. Client disconnected.");
                this.Disconnect("Incomplete packet.");
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
            this.ServerSession.WritePacket(data);
        }

        /// <summary>
        /// Immediately disconnects the client from the server.
        /// </summary>
        /// <param name="reason">The reason for the disconnection.</param>
        public void Disconnect(string reason = null)
        {
            var reasonString = string.IsNullOrWhiteSpace(reason) ? "(no reason supplied)" : reason;

            this.LogDisconnectReason(this.AccountSession, reasonString);
            this.ServerSession.Close(reasonString);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                Misc.AssignNullAndDispose(ref this.accountSession);

                this.keepAliveTimer.Close();

                this.ServerSession.Close("Client disposed.");

                this.isDisposed = true;
            }
        }

        private void LogDisconnectReason(IAccountSession session, string reason)
        {
            if (session != null)
            {
                this.Logger.Debug("Account session #{0} was closed: {1}", session.SessionId, reason);
            }
            else
            {
                this.Logger.Debug("Session was closed: {0}", reason);
            }
        }
    }
}
