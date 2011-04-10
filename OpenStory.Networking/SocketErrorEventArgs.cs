using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// An EventArgs wrapper around <see cref="SocketError"/>.
    /// </summary>
    public class SocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the SocketErrorEventArgs class.
        /// </summary>
        /// <param name="error">The SocketError to wrap around.</param>
        public SocketErrorEventArgs(SocketError error)
        {
            this.Error = error;
        }

        /// <summary>
        /// Gets the wrapped SocketError.
        /// </summary>
        public SocketError Error { get; private set; }
    }
}