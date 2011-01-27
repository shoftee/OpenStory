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
        /// <summary>
        /// The event raised when a new socket connection has been accepted.
        /// </summary>
        public event EventHandler<SocketEventArgs> OnSocketAccepted;

        private readonly Socket acceptSocket;
        private readonly SocketAsyncEventArgs socketArgs;

        /// <summary>
        /// Initializes a new instance of SocketAcceptor and binds it to the given port.
        /// </summary>
        /// <param name="port">The port to bind this SocketAcceptor to.</param>
        public SocketAcceptor(int port)
        {
            this.Port = port;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += (sender, eventArgs) => this.EndAccept(eventArgs);

            this.acceptSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// The port to which this SocketAcceptor is bound.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Starts the process of accepting connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="OnSocketAccepted"/> 
        /// event has no subscribers.
        /// </exception>
        public void Start()
        {
            if (OnSocketAccepted == null)
            {
                throw new InvalidOperationException("'OnSocketAccepted' has no subscribers.");
            }
            var localEndPoint = new IPEndPoint(IPAddress.Any, this.Port);
            this.acceptSocket.Bind(localEndPoint);
            this.acceptSocket.Listen(100);

            this.BeginAccept();
        }

        private void BeginAccept()
        {
            this.socketArgs.AcceptSocket = null;

            bool asynchronous = this.acceptSocket.AcceptAsync(this.socketArgs);
            if (!asynchronous)
            {
                this.EndAccept(this.socketArgs);
            }
        }

        private void EndAccept(SocketAsyncEventArgs eventArgs)
        {
            Socket clientSocket = eventArgs.AcceptSocket;
            var socketEventArgs = new SocketEventArgs(clientSocket);
            this.OnSocketAccepted(this, socketEventArgs);
            this.BeginAccept();
        }
    }
}