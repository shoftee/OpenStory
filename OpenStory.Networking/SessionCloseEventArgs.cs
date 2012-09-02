using System;

namespace OpenStory.Networking
{
    internal class SessionCloseEventArgs : EventArgs
    {
        public static readonly SessionCloseEventArgs RemoteHostDisconnected =
            new SessionCloseEventArgs("The remote host disconnected.");

        public static readonly SessionCloseEventArgs ConnectionTerminated =
            new SessionCloseEventArgs("The connection was forcibly terminated.");

        public SessionCloseEventArgs(string reason)
        {
            this.Reason = reason;
        }

        public string Reason { get; private set; }
    }
}
