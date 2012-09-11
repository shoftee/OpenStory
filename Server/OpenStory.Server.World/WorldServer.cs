using System;
using System.Collections.Generic;

namespace OpenStory.Server.World
{
    internal class WorldServer : IWorldServer, IChannelWorld
    {
        /// <inheritdoc />
        public int WorldId { get; private set; }

        private readonly Dictionary<int, IWorldChannel> channels;

        public WorldServer(int worldId)
        {
            this.WorldId = worldId;

            this.channels = new Dictionary<int, IWorldChannel>();
        }

        /// <inheritdoc />
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
