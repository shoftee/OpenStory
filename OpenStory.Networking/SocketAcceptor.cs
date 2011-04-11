using System;
using System.Net;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a simple connection acceptor.
    /// </summary>
    public class SocketAcceptor
    {
        private readonly IPEndPoint localEndPoint;
        private SocketAsyncEventArgs socketArgs;
        private Socket acceptSocket;

        /// <summary>
        /// Initializes a new instance of SocketAcceptor and binds it to the given port.
        /// </summary>
        /// <param name="port">The port to bind this SocketAcceptor to.</param>
        public SocketAcceptor(int port)
        {
            this.Port = port;

            this.localEndPoint = new IPEndPoint(IPAddress.Any, this.Port);
        }

        /// <summary>
        /// The port to which this SocketAcceptor is bound.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// The event raised when a new socket connection has been accepted.
        /// </summary>
        public event EventHandler<SocketEventArgs> OnSocketAccepted;

        /// <summary>
        /// The event raised when there's a socket error.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> OnSocketError;

        private Socket GetAcceptSocket()
        {
            if (this.acceptSocket != null)
            {
                this.acceptSocket.Dispose();
            }

            if (this.socketArgs != null)
            {
                this.socketArgs.Dispose();
            }

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += (sender, eventArgs) => this.EndAcceptAsynchronous(eventArgs);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(this.localEndPoint);
            socket.Listen(100);

            return socket;
        }

        /// <summary>
        /// Starts the process of accepting connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="OnSocketAccepted"/> event has no subscribers.
        /// </exception>
        public void Start()
        {
            if (this.OnSocketAccepted == null)
            {
                throw new InvalidOperationException("'OnSocketAccepted' has no subscribers.");
            }

            this.acceptSocket = this.GetAcceptSocket();

            this.BeginAccept();
        }

        /// <summary>
        /// Halts the process of accepting connections.
        /// </summary>
        public void Stop()
        {
            this.acceptSocket.Close();
            this.acceptSocket = null;
        }

        private void BeginAccept()
        {
            this.socketArgs.AcceptSocket = null;

            while (!this.acceptSocket.AcceptAsync(this.socketArgs))
            {
                if (!this.EndAcceptSynchronous(this.socketArgs)) break;
            }
        }

        private bool EndAcceptSynchronous(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs.SocketError != SocketError.Success)
            {
                this.HandleError(eventArgs.SocketError);
                return false;
            }

            Socket clientSocket = eventArgs.AcceptSocket;
            var socketEventArgs = new SocketEventArgs(clientSocket);
            this.OnSocketAccepted(this, socketEventArgs);
            return true;
        }

        private void EndAcceptAsynchronous(SocketAsyncEventArgs eventArgs)
        {
            if (this.EndAcceptSynchronous(eventArgs))
            {
                this.BeginAccept();
            }
        }

        private void HandleError(SocketError error)
        {
            if (this.OnSocketError != null)
            {
                this.OnSocketError(this, new SocketErrorEventArgs(error));
            }

            // OperationAborted comes up when we closed the socket manually.
            if (error != SocketError.OperationAborted)
            {
                this.Stop();
            }
        }
    }
}