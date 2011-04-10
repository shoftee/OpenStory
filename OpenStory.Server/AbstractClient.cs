using System;
using System.Timers;
using OpenStory.Common;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// This class is abstract.
    /// </summary>
    public abstract class AbstractClient
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

        private Timer keepAliveTimer;
        private AtomicInteger sentPings;
        private static readonly byte[] PingPacket = new byte[] { 0x0F, 0x00 };

        /// <summary>
        /// Initializes a new client with the given network session object.
        /// </summary>
        /// <param name="session">The session object for this client.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="session"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="session"/> is not open.</exception>
        protected AbstractClient(ServerSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (session.SessionId == -1) throw new InvalidOperationException("This session is not open.");

            this.Session = session;
            this.Session.OnPacketReceived += this.HandlePacket;

            this.AccountSession = null;

            this.keepAliveTimer = new Timer(PingInterval);
            this.keepAliveTimer.Elapsed += this.HandlePing;

            this.sentPings = new AtomicInteger(0);
            this.keepAliveTimer.Start();
        }

        private void HandlePing(object sender, ElapsedEventArgs e)
        {
            Log.WriteInfo("PING {0}", this.sentPings.Value);
            if (this.sentPings.Increment() > PingsAllowed)
            {
                this.Disconnect();
                return;
            }
            this.Session.WritePacket(ByteHelpers.CloneArray(PingPacket));
        }

        void HandlePacket(object sender, IncomingPacketEventArgs e)
        {
            PacketReader reader = e.Reader;
            ushort opCode = reader.ReadUInt16();
            if (opCode == 0x11)
            {
                this.sentPings.ExchangeWith(0);
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

        /// <summary>
        /// Immediately disconnects the client from the server.
        /// </summary>
        public void Disconnect()
        {
            if (this.AccountSession != null)
            {
                this.AccountSession.Dispose();
                this.AccountSession = null;
            }
            this.keepAliveTimer.Dispose();
            this.Session.Close();
        }
    }
}