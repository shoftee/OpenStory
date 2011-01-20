using System;
using System.Net;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents a simple connection acceptor.
    /// </summary>
    public class Acceptor
    {
        private readonly Action<Socket> onAccept;
        private readonly Socket socket;
        private readonly SocketAsyncEventArgs socketArgs;

        /// <summary>
        /// Initializes a new instance of Acceptor and binds it to the given port.
        /// </summary>
        /// <param name="port">The port to bind this Acceptor to.</param>
        /// <param name="onAccept">The function to call when a new connection is made.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="onAccept"/> is <c>null</c>.</exception>
        public Acceptor(int port, Action<Socket> onAccept)
        {
            if (onAccept == null) throw new ArgumentNullException("onAccept");
            this.onAccept = onAccept;

            this.Port = port;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += (sender, eventArgs) => this.EndAccept(eventArgs);

            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>The port to which this Acceptor is bound.</summary>
        public int Port { get; private set; }

        /// <summary>Starts the process of accepting connections.</summary>
        public void Start()
        {
            var localEndPoint = new IPEndPoint(IPAddress.Any, this.Port);
            this.socket.Bind(localEndPoint);
            this.socket.Listen(100);
            this.BeginAccept();
        }

        private void BeginAccept()
        {
            this.socketArgs.AcceptSocket = null;

            bool asynchronous = this.socket.AcceptAsync(this.socketArgs);
            if (!asynchronous)
            {
                this.EndAccept(this.socketArgs);
            }
        }

        private void EndAccept(SocketAsyncEventArgs eventArgs)
        {
            Socket clientSocket = eventArgs.AcceptSocket;
            this.onAccept(clientSocket);
            this.BeginAccept();
        }
    }
}