using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace OpenMaple.Networking
{
    delegate void SocketAcceptCallback(Socket socket);

    /// <summary>
    /// Represents a simple connection acceptor.
    /// </summary>
    class Acceptor
    {
        /// <summary>
        /// The port to which this Acceptor is bound.
        /// </summary>
        public int Port { get; private set; }

        private readonly SocketAcceptCallback acceptCallback;
        private readonly Socket socket;

        /// <summary>
        /// Initializes a new instance of Acceptor and binds it to the given port.
        /// </summary>
        /// <param name="port">The port to bind this Acceptor to.</param>
        /// <param name="acceptCallback">The function to call when a new connection is made.</param>
        public Acceptor(int port, SocketAcceptCallback acceptCallback)
        {
            this.Port = port;
            this.acceptCallback = acceptCallback;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress localhostAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(localhostAddress, Port);
            this.socket.Bind(localEndPoint);
        }

        /// <summary>
        /// Starts the process of accepting connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">The exception is thrown when the OnAccept event has no subscribers.</exception>
        public void Start()
        {
            this.socket.Listen(100);
            this.BeginAccept(null);
        }

        /// <summary>
        /// Does the asynchronous accept, in a very nice and efficient manner :D
        /// </summary>
        /// <param name="args">The SocketAsyncEventArgs object to use or reuse.</param>
        private void BeginAccept(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                args = new SocketAsyncEventArgs();
                args.Completed += (sender, eventArgs) => this.EndAccept(eventArgs);
            }
            else
            {
                args.AcceptSocket = null;
            }

            bool isAsynchronous = this.socket.AcceptAsync(args);
            if (!isAsynchronous)
            {
                this.EndAccept(args);
            }
        }

        private void EndAccept(SocketAsyncEventArgs eventArgs)
        {
            Socket clientSocket = eventArgs.AcceptSocket;
            this.acceptCallback(clientSocket);
            this.BeginAccept(eventArgs);
        }
    }
}
