using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.World
{
    class WorldServer : IWorldServer, IChannelWorld
    {
        public int WorldId { get; private set; }

        private Dictionary<int, IWorldChannel> channels;

        public WorldServer(int worldId)
        {
            this.WorldId = worldId;

            this.channels = new Dictionary<int, IWorldChannel>();
        }

        public void BroadcastFromChannel(int channelId, int[] targets, byte[] data)
        {
            throw new NotImplementedException();
        }

        public IWorldChannel GetChannelById(int channelId)
        {
            IWorldChannel channel;
            bool success = this.channels.TryGetValue(channelId, out channel);
            return success ? channel : null;
        }
    }
}
