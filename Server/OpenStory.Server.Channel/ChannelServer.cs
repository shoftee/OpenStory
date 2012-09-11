using System.Collections.Generic;
using System.Linq;

namespace OpenStory.Server.Channel
{
    internal class ChannelServer : IChannelServer
    {
        private readonly int channelId;

        private readonly PlayerRegistry players;

        public IChannelWorld World { get; private set; }

        public ChannelServer(IChannelWorld worldServer, int channelId)
        {
            this.World = worldServer;
            this.channelId = channelId;

            this.players = new PlayerRegistry();
        }

        public void BroadcastToWorld(int sourceId, IEnumerable<int> targetIds, byte[] data)
        {
            var ids = from id in targetIds
                      where id != sourceId
                      select id;

            this.BroadcastToWorld(ids, data);
        }

        private void BroadcastToWorld(IEnumerable<int> targetIds, byte[] data)
        {
            // Arrays are more efficient for remoting operations.
            int[] ids = targetIds.ToArray();

            this.BroadcastIntoChannel(players.GetActive(ids), data);

            this.World.BroadcastFromChannel(this.channelId, ids, data);
        }

        // This method will be part of the service contract.
        /// <summary>
        /// Broadcasts a packet to the targets that reside in the current channel.
        /// </summary>
        /// <param name="targetIds"></param>
        /// <param name="data"></param>
        public void BroadcastIntoChannel(IEnumerable<int> targetIds, byte[] data)
        {
            var playerTargets = from targetId in targetIds
                                select this.players.GetById(targetId)
                                into target
                                where target != null
                                select target;

            foreach (Player player in playerTargets)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
