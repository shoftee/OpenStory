using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace OpenMaple.Networking
{
    /// <summary>Represents a simple connection acceptor.</summary>
    class Acceptor
    {
        /// <summary>The port to which this Acceptor is bound.</summary>
        public int Port { get; private set; }

        private readonly Socket socket;
        private readonly SocketAsyncEventArgs socketArgs;

        private readonly Action<Socket> onAccept;

        /// <summary>Initializes a new instance of Acceptor and binds it to the given port.</summary>
        /// <param name="port">The port to bind this Acceptor to.</param>
        /// <param name="onAccept">The function to call when a new connection is made.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown if <paramref name="onAccept"/> is null.</exception>
        public Acceptor(int port, Action<Socket> onAccept)
        {
            if (onAccept == null) throw new ArgumentNullException("onAccept");
            this.onAccept = onAccept;

            this.Port = port;

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += (sender, eventArgs) => this.EndAccept(eventArgs);
            
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

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
            socketArgs.AcceptSocket = null;

            bool asynchronous = this.socket.AcceptAsync(socketArgs);
            if (!asynchronous)
            {
                this.EndAccept(socketArgs);
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
