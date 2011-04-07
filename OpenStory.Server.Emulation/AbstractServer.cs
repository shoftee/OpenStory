using System;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Common.Tools;
using OpenStory.Cryptography;
using OpenStory.Networking;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// A base class for Server instances.
    /// </summary>
    abstract class AbstractServer
    {
        private static readonly ushort MapleVersion = Properties.Settings.Default.MapleVersion;

        private SocketAcceptor acceptor;

        protected abstract string ServerName { get; }

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

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
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.ThrowIfRunning();
            this.IsRunning = true;
            this.StartAccepting();
        }

        private void StartAccepting()
        {
            Log.WriteInfo("[{0}] Listening on port {1}.", this.ServerName, acceptor.Port);
            this.acceptor.Start();
        }

        /// <summary>
        /// Shuts down the server after the given delay.
        /// </summary>
        /// <param name="delay">The time to wait before initiating the shut down procedure.</param>
        public void ShutDown(TimeSpan delay)
        {
            this.ThrowIfNotRunning();
            throw new NotImplementedException();
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
                throw new InvalidOperationException("The server has not been started. Call the Start method before using it.");
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

        /// <summary>
        /// This method is called when a new client session has been initialized.
        /// </summary>
        /// <remarks>
        /// An internal reference to the session is not kept, when 
        /// overriding this method be sure to save a reference to it.
        /// </remarks>
        /// <param name="serverSession">The new session to process.</param>
        protected abstract void HandleSession(ServerSession serverSession);

        private void HandleAccept(Socket socket)
        {
            var clientIV = ByteHelpers.GetNewIV();
            var serverIV = ByteHelpers.GetNewIV();
            ServerCrypto crypto = new ServerCrypto(clientIV, serverIV, MapleVersion);
            ServerSession serverSession = new ServerSession(socket, crypto);
            serverSession.OnClosing += HandleSessionClose;

            byte[] helloPacket = ConstructHelloPacket(clientIV, serverIV);
            this.HandleSession(serverSession);

            Log.WriteInfo("Session {0} started : CIV {1} SIV {2}.", serverSession.SessionId, BitConverter.ToString(clientIV), BitConverter.ToString(serverIV));

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

        private static void HandleSessionClose(object sender, EventArgs args)
        {
            ServerSession serverSession = sender as ServerSession;
            if (serverSession == null) return;

            Log.WriteInfo("Session {0} closed.", serverSession.SessionId);
        }
    }
}
