using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using OpenStory.Networking;

namespace OpenStory.Server.Networking
{
    /// <summary>
    /// Represents a simple connection acceptor.
    /// </summary>
    [Localizable(true)]
    public sealed class SocketAcceptor : IDisposable
    {
        /// <summary>
        /// Occurs when a new socket connection has been accepted.
        /// </summary>
        public event EventHandler<SocketEventArgs> SocketAccepted;

        /// <summary>
        /// Occurs when a socket error occurs.
        /// </summary>
        public event EventHandler<SocketErrorEventArgs> SocketError;

        private SocketAsyncEventArgs socketArgs;
        private Socket acceptSocket;
        private bool isDisposed;

        /// <summary>
        /// Gets the bound endpoint for the acceptor.
        /// </summary>
        public IPEndPoint Endpoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketAcceptor"/> class.
        /// </summary>
        /// <param name="endpoint">The <see cref="IPEndPoint"/> to accept connections through.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="endpoint"/> is <see langword="null"/>.
        /// </exception>
        public SocketAcceptor(IPEndPoint endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException("endpoint");
            }

            this.isDisposed = false;

            this.Endpoint = endpoint;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.DisposeSocketIfNotNull();
                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Starts the process of accepting connections.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the <see cref="SocketAccepted"/> event has no subscribers.
        /// </exception>
        public void Start()
        {
            if (this.SocketAccepted == null)
            {
                throw new InvalidOperationException(CommonStrings.AcceptEventHasNoSubscribers);
            }

            this.acceptSocket = this.GetAcceptSocket();

            this.BeginAccept();
        }

        /// <summary>
        /// Halts the process of accepting connections.
        /// </summary>
        public void Stop()
        {
            this.DisposeSocketIfNotNull();
        }

        private Socket GetAcceptSocket()
        {
            this.DisposeSocketIfNotNull();

            this.socketArgs = new SocketAsyncEventArgs();
            this.socketArgs.Completed += (o, e) => this.EndAcceptAsynchronous(e);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(this.Endpoint);
            socket.Listen(100);

            return socket;
        }

        private void BeginAccept()
        {
            this.socketArgs.AcceptSocket = null;

            // For the confused: AcceptAsync returns false if the operation completed synchronously.
            // As long as the operation completes synchronously, we can handle the socket synchronously too.
            while (!this.acceptSocket.AcceptAsync(this.socketArgs))
            {
                if (!this.EndAcceptSynchronous(this.socketArgs))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Handles a synchronous socket accept operation. 
        /// </summary>
        /// <param name="eventArgs">The <see cref="SocketAsyncEventArgs"/> instance containing the accepted socket.</param>
        /// <returns><see langword="true"/> if the socket was handled successfully; otherwise, <see langword="false"/>.</returns>
        private bool EndAcceptSynchronous(SocketAsyncEventArgs eventArgs)
        {
            if (eventArgs.SocketError != System.Net.Sockets.SocketError.Success)
            {
                this.OnSocketError(eventArgs.SocketError);
                return false;
            }

            this.OnSocketAccepted(eventArgs.AcceptSocket);
            return true;
        }

        private void OnSocketAccepted(Socket acceptedSocket)
        {
            this.SocketAccepted(this, new SocketEventArgs(acceptedSocket));
        }

        /// <summary>
        /// Handles an asynchronous socket accept operation.
        /// </summary>
        /// <remarks>
        /// The only difference between this and <see cref="EndAcceptSynchronous"/> is 
        /// that this calls <see cref="BeginAccept"/> if the socket was handled successfully.
        /// </remarks>
        /// <param name="eventArgs">The <see cref="SocketAsyncEventArgs"/> instance containing the accepted socket.</param>
        private void EndAcceptAsynchronous(SocketAsyncEventArgs eventArgs)
        {
            if (this.EndAcceptSynchronous(eventArgs))
            {
                this.BeginAccept();
            }
        }

        private void OnSocketError(SocketError error)
        {
            var handler = this.SocketError;
            if (handler != null)
            {
                handler(this, new SocketErrorEventArgs(error));
            }

            // OperationAborted comes up when we closed the socket manually.
            if (error != System.Net.Sockets.SocketError.OperationAborted)
            {
                this.Stop();
            }
        }

        private void DisposeSocketIfNotNull()
        {
            if (this.acceptSocket != null)
            {
                this.acceptSocket.Dispose();
            }

            if (this.socketArgs != null)
            {
                this.socketArgs.Dispose();
            }
        }
    }
}
