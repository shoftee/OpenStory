using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// Represents an EventArgs wrapper around a Socket.
    /// </summary>
    public class SocketEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the socket of this instance.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SocketEventArgs"/>.
        /// </summary>
        /// <param name="socket">The socket for this instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="socket"/> is <c>null</c>.</exception>
        public SocketEventArgs(Socket socket)
        {
            if (socket == null)
            {
                throw new ArgumentNullException("socket");
            }
            this.Socket = socket;
        }
    }
}