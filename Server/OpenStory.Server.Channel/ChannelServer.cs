using System.Collections.Generic;
using System.Linq;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Registry;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a server that handles the channel-side operations.
    /// </summary>
    internal sealed class ChannelServer : IChannelServer
    {
        private readonly IPlayerRegistry playerRegistry;

        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        public int WorldId { get; private set; }

        /// <inheritdoc />
        public IChannelWorld World { get; private set; }

        /// <inheritdoc />
        public ChannelServer(
            ChannelConfiguration configuration,
            IPlayerRegistry playerRegistry)
        {
            this.ChannelId = configuration.ChannelId;
            this.WorldId = configuration.WorldId;

            this.playerRegistry = playerRegistry;
        }

        /// <inheritdoc />
        public void BroadcastToWorld(CharacterKey sourceKey, IEnumerable<CharacterKey> targets, byte[] data)
        {
            var ids = from id in targets
                      where id != sourceKey
                      select id;

            this.BroadcastToWorld(ids, data);
        }

        private void BroadcastToWorld(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // Arrays are more efficient for remote operations.
            CharacterKey[] keys = targets.ToArray();

            this.BroadcastIntoChannel(this.playerRegistry.Scan(keys).Select(p => p.Key), data);
            this.World.BroadcastFromChannel(this.ChannelId, keys, data);
        }

        /// <summary>
        /// Broadcasts a packet to the targets that reside in the current channel.
        /// </summary>
        /// <param name="targets">The characters to broadcast to.</param>
        /// <param name="data">The packet payload to broadcast.</param>
        public void BroadcastIntoChannel(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // This method will be part of the service contract.
            var targetPlayers = from target in playerRegistry.Scan(targets)
                                select target;

            foreach (var player in targetPlayers)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
