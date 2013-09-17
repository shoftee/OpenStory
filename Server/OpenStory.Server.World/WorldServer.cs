using System.Collections.Generic;
using System.Linq;
using OpenStory.Framework.Model.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    internal class WorldServer : IChannelWorldRequestHandler
    {
        /// <inheritdoc />
        public int WorldId { get; private set; }

        private readonly Dictionary<int, IWorldChannelRequestHandler> channels;

        public WorldServer(int worldId)
        {
            this.WorldId = worldId;

            this.channels = new Dictionary<int, IWorldChannelRequestHandler>();
        }

        /// <inheritdoc />
        public void BroadcastFromChannel(int channelId, CharacterKey[] targets, byte[] data)
        {
            var handlers = from entry in this.channels
                           where entry.Key != channelId
                           select entry.Value;

            foreach (var handler in handlers.ToArray())
            {
                handler.BroadcastIntoChannel(targets, data);
            }
        }
    }
}
