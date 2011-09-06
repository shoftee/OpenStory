using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Channel
{
    class ChannelServer : IChannelServer
    {
        private int channelId;

        private PlayerRegistry players;

        public IChannelWorld World { get; private set; }

        public ChannelServer(IChannelWorld worldServer, int channelId)
        {
            this.World = worldServer;
            this.channelId = channelId;

            this.players = new PlayerRegistry();
        }

        public void BroadcastToWorld(int sourceId, IEnumerable<int> targets, byte[] data)
        {
            this.BroadcastToWorld(targets.Where(i => i != sourceId), data);
        }

        private void BroadcastToWorld(IEnumerable<int> targets, byte[] data)
        {
            // Arrays are more efficient for remoting operations.
            int[] ids = targets.ToArray();

            this.BroadcastIntoChannel(players.GetActive(ids), data);
            
            this.World.BroadcastFromChannel(this.channelId, ids, data);
        }

        public void BroadcastIntoChannel(IEnumerable<int> targets, byte[] data)
        {
            var playerTargets = 
                targets
                .Select(id => this.players.GetById(id))
                .Where(player => player != null);
            foreach (Player player in playerTargets)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
