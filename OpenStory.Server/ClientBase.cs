using System;
using System.Globalization;
using System.Timers;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Networking;
using OpenStory.Server.Fluent;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// This class is abstract.
    /// </summary>
    public abstract class ClientBase
    {
        /// <summary>
        /// The number of pings a client is allowed to miss before being disconnected.
        /// </summary>
        private const int PingsAllowed = 3;

        /// <summary>
        /// The period between pings, in milliseconds.
        /// </summary>
        private const int PingInterval = 15000;

        /// <summary>
        /// The op code for the pong packet.
        /// </summary>
        protected const int PongOpCode = 0x0011;

        /// <summary>
        /// Gets the client's session object.
        /// </summary>
        protected ServerSession Session { get; private set; }

        /// <summary>
        /// Gets the remote address for this session.
        /// </summary>
        public string RemoteAddress { get; private set; }

        /// <summary>
        /// Gets the account session object.
        /// </summary>
        /// <remarks>
        /// This object is null if the client has not logged in.
        /// </remarks>
        public IAccountSession AccountSession { get; protected set; }

        private readonly Timer keepAliveTimer;
        private readonly AtomicInteger sentPings;
        private static readonly byte[] PingPacket = new byte[] { 0x0F, 0x00 };

        /// <summary>
        /// Initializes a new instance of <see cref="ClientBase"/>.
        /// </summary>
        /// <param name="session">The session object for this client.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="session"/> is <c>null</c>.
        /// </exception>
        protected ClientBase(ServerSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            this.Session = session;
            this.Session.PacketReceived += this.OnPacketReceived;

            this.AccountSession = null;

            this.keepAliveTimer = new Timer(PingInterval);
            this.keepAliveTimer.Elapsed += this.HandlePing;

            this.sentPings = new AtomicInteger(0);
            this.keepAliveTimer.Start();
        }

        #region Packet handling

        private void HandlePing(object sender, ElapsedEventArgs e)
        {
            OS.Log().Info("PING {0}", this.sentPings.Value);
            if (this.sentPings.Increment() > PingsAllowed)
            {
                this.Disconnect("No ping response.");
                return;
            }
            this.Session.WritePacket(PingPacket);
        }

        private void OnPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var reader = e.Reader;

            ushort opCode;
            if (!reader.TryReadUInt16(out opCode))
            {
                this.Disconnect("No packet op code.");
                return;
            }

            if (opCode == PongOpCode)
            {
                TimeSpan lag;
                bool alive = this.AccountSession.TryKeepAlive(out lag);
                if (!alive)
                {
                    this.Disconnect("Session keep-alive failed.");
                }
                else
                {
                    this.sentPings.ExchangeWith(0);
                }
            }
            else
            {
                this.ProcessPacket(opCode, reader);
            }
        }

        /// <summary>
        /// When implemented in a derived class, processes the packet with the given op code.
        /// </summary>
        /// <param name="opCode">The op code of the packet to process.</param>
        /// <param name="reader">A <see cref="PacketReader"/> object for the packet.</param>
        protected abstract void ProcessPacket(ushort opCode, PacketReader reader);

        #endregion

        /// <summary>
        /// Writes a packet to the client's stream.
        /// </summary>
        /// <param name="data">The data of the packet.</param>
        public void WritePacket(byte[] data)
        {
            this.Session.WritePacket(data);
        }

        /// <summary>
        /// Immediately disconnects the client from the server.
        /// </summary>
        /// <param name="reason">The reason for the disconnection.</param>
        public void Disconnect(string reason = null)
        {
            LogDisconnectReason(this.AccountSession, reason);

            if (this.AccountSession != null)
            {
                this.AccountSession.Dispose();
                this.AccountSession = null;
            }
            this.keepAliveTimer.Dispose();
            this.Session.Close();
        }

        private static void LogDisconnectReason(IAccountSession session, string reason)
        {
            var sessionIdString = session != null ? session.SessionId.ToString(CultureInfo.InvariantCulture) : "(N/A)";
            var reasonString = String.IsNullOrWhiteSpace(reason) ? "(no reason supplied)" : reason;

            OS.Log().Info("Session #{0} was closed: {1}", sessionIdString, reasonString);
        }
    }
}
