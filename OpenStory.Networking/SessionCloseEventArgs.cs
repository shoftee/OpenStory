using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Networking
{
    class SessionCloseEventArgs : EventArgs
    {
        public static readonly SessionCloseEventArgs RemoteHostDisconnected = new SessionCloseEventArgs("The remote host disconnected.");
        public static readonly SessionCloseEventArgs ConnectionTerminated = new SessionCloseEventArgs("The connection was forcibly terminated.");

        public string Reason { get; private set; }

        public SessionCloseEventArgs(string reason)
        {
            this.Reason = reason;
        }
    }
}
