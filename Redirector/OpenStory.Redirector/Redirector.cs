using System;
using System.Net;
using System.Net.Sockets;
using OpenStory.Common.IO;
using OpenStory.Networking;
using OpenStory.Redirector.Connection;

namespace OpenStory.Redirector
{
    internal sealed class Redirector : IDisposable
    {
        private bool isDisposed;

        private readonly SocketAcceptor initialAcceptor;

        private SocketAcceptor channelAcceptor;

        private ServerSession clientLink;
        private ClientSession serverLink;

        public Redirector(IPEndPoint endpoint)
        {
            this.initialAcceptor = new SocketAcceptor(endpoint);
            this.PassEndpointToSocketsFrom(this.initialAcceptor, endpoint);
        }

        public void Bind()
        {
            var acceptor = this.initialAcceptor;
            BindInternal(acceptor);
        }

        private static void BindInternal(SocketAcceptor acceptor)
        {
            Logger.Write(LogMessageType.Connection, "Listening for clients on port {0}.", acceptor.Endpoint.Port);

            acceptor.Start();
        }

        private void PassEndpointToSocketsFrom(SocketAcceptor acceptor, IPEndPoint endpoint)
        {
            EventHandler<SocketEventArgs> handler = (s, e) =>
            {
                Logger.Write(LogMessageType.Connection, "Client connection to port {0} intercepted.", endpoint.Port);
                this.AcceptClient(e.Socket, endpoint);
            };

            acceptor.SocketAccepted += handler;
        }

        private void AcceptClient(Socket clientSocket, IPEndPoint endpoint)
        {
            this.clientLink = new ServerSession();
            this.clientLink.PacketReceived += this.HandleClientPacketReceived;
            this.clientLink.Closing += this.HandleClientClosing;
            this.clientLink.AttachSocket(clientSocket);

            this.serverLink = new ClientSession();
            this.serverLink.HandshakeReceived += this.HandleHandshakeReceived;
            this.serverLink.PacketReceived += this.HandleServerPacketReceived;

            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            this.serverLink.AttachSocket(serverSocket);
            Logger.Write(LogMessageType.Connection, "Connecting to server {0}", endpoint);
            serverSocket.Connect(endpoint);
            Logger.Write(LogMessageType.Connection, "Successfully connected to server {0}", endpoint);
            this.serverLink.Start();
        }

        private void HandleClientClosing(object sender, EventArgs e)
        {
            Logger.Write(LogMessageType.Connection, "The client closed the connection.");

            this.serverLink.Close();
        }

        private void HandleHandshakeReceived(object sender, HandshakeReceivedEventArgs e)
        {
            var factory = Helpers.GetFactoryForVersion(e.Info.Version);
            this.clientLink.Start(factory, e.Info);
        }

        private void HandleServerPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            var reader = (IUnsafePacketReader)e.Reader;
            if (reader.ReadUInt16() == 0x0C)
            {
                // Handle channel change <3
                var builder = new PacketBuilder();
                builder.WriteBytes(reader.ReadBytes(4));

                string ip = String.Join(".", reader.ReadBytes(4));
                ushort port = reader.ReadUInt16();

                // Loopback is 127.0.0.1
                // GetAddressBytes will return {127, 0, 0, 1}
                var bytes = IPAddress.Loopback.GetAddressBytes();

                builder.WriteBytes(bytes);
                builder.WriteInt16(port);
                builder.WriteBytes(reader.ReadBytes(14));
                this.serverLink.Close();

                if (port != this.initialAcceptor.Endpoint.Port)
                {
                    var endpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                    GetNewChannelAcceptor(ref this.channelAcceptor, endpoint);
                    this.PassEndpointToSocketsFrom(this.channelAcceptor, endpoint);

                    BindInternal(this.channelAcceptor);
                }

                this.clientLink.WritePacket(builder.ToByteArray());
            }
            else
            {
                this.clientLink.WritePacket(e.Reader.ReadFully());
            }
        }

        private static void GetNewChannelAcceptor(ref SocketAcceptor acceptor, IPEndPoint endpoint)
        {
            if (acceptor != null)
            {
                acceptor.Dispose();
            }

            acceptor = new SocketAcceptor(endpoint);
        }

        private void HandleClientPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            this.serverLink.WritePacket(e.Reader.ReadFully());
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            if (this.isDisposed)
            {
                var localAcceptor = this.initialAcceptor;
                if (localAcceptor != null) localAcceptor.Dispose();

                localAcceptor = this.channelAcceptor;
                if (localAcceptor != null) localAcceptor.Dispose();

                this.isDisposed = true;
            }
        }

        #endregion
    }
}
