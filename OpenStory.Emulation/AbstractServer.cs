using System;
using System.Net.Sockets;
using OpenStory.Networking;

namespace OpenStory.Emulation
{
    /// <summary>
    /// A base class for Server instances.
    /// </summary>
    public abstract class AbstractServer
    {
        private Acceptor connectionAcceptor;

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
            connectionAcceptor = new Acceptor(port, HandleAccept);
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            this.IsRunning = true;
            this.StartAccepting();
        }

        private void StartAccepting()
        {
            connectionAcceptor.Start();
        }

        /// <summary>
        /// Shuts down the server after the given delay.
        /// </summary>
        /// <param name="delay">The time to wait before initiating the shut down procedure.</param>
        public void ShutDown(TimeSpan delay)
        {
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
            if (!IsRunning)
            {
                throw new InvalidOperationException("The server has not been started. Call the Start method before using it.");
            }
        }

        /// <summary>
        /// This method is a callback for the internal socket acceptor.
        /// </summary>
        /// <param name="socket">The socket to handle the connection of.</param>
        protected abstract void HandleAccept(Socket socket);
    }
}
