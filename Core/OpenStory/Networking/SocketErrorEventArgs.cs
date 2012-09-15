using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    /// <summary>
    /// An EventArgs wrapper around <see cref="SocketError"/>.
    /// </summary>
    public sealed class SocketErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the wrapped SocketError.
        /// </summary>
        public SocketError Error { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="SocketErrorEventArgs"/>.
        /// </summary>
        /// <param name="error">The <see cref="SocketError"/> to wrap.</param>
        public SocketErrorEventArgs(SocketError error)
        {
            this.Error = error;
        }
    }
}
