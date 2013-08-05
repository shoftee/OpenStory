using System;
using System.Threading.Tasks;
using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    internal sealed class PacketScheduler : IPacketScheduler
    {
        public void Register(IServerSession session)
        {
            session.ReadyForPush += this.OnReadyForPush;
        }

        private void OnReadyForPush(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => ((IServerSession)sender).Push());
        }
    }
}
