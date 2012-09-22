using System;
using OpenStory.Common.IO;

namespace OpenStory.Redirector.Connection
{
    internal sealed class HandshakeReceivedEventArgs : EventArgs
    {
        public HandshakeInfo Info { get; private set; }

        public HandshakeReceivedEventArgs(HandshakeInfo info)
        {
            this.Info = info;
        }
    }
}