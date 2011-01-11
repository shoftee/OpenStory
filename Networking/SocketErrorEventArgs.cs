using System;
using System.Net.Sockets;

namespace OpenMaple.Networking
{
    class SocketErrorEventArgs : EventArgs
    {
        public SocketError Error { get; private set; }

        public SocketErrorEventArgs(SocketError error)
        {
            this.Error = error;
        }
    }
}