using System;

namespace OpenStory.Framework.Contracts
{
    public class ServerSessionEventArgs : EventArgs
    {
        public IServerSession ServerSession { get; private set; }

        public ServerSessionEventArgs(IServerSession serverSession)
        {
            this.ServerSession = serverSession;
        }
    }
}