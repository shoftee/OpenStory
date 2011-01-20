using System;
using System.Net.Sockets;

namespace OpenStory.Networking
{
    internal class SocketErrorEventArgs : EventArgs
    {
        public SocketErrorEventArgs(SocketError error)
        {
            this.Error = error;
        }

        public SocketError Error { get; private set; }
    }
}