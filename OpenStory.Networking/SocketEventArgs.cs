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
        /// Gets the socket of this SocketEventArgs object.
        /// </summary>
        public Socket Socket { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SocketEventArgs class.
        /// </summary>
        /// <param name="socket">The socket for this instance.</param>
        public SocketEventArgs(Socket socket)
        {
            this.Socket = socket;
        }
    }
}