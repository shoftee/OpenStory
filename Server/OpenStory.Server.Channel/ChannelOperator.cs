using System.Collections.Generic;
using System.Linq;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;
using OpenStory.Server.Registry;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a server that handles the channel-side operations.
    /// </summary>
    public sealed class ChannelOperator : ServerOperator<ChannelClient>, IChannelOperator
    {
        private readonly IPlayerRegistry playerRegistry;

        private ChannelConfiguration channelConfiguration;

        /// <summary>
        /// Gets the channel identifier.
        /// </summary>
        public int ChannelId { get; private set; }

        /// <summary>
        /// Gets the world identifier.
        /// </summary>
        public int WorldId { get; private set; }

        /// <inheritdoc />
        public IChannelWorldRequestHandler World { get; private set; }

        /// <inheritdoc />
        public ChannelOperator(
            IGameClientFactory<ChannelClient> clientFactory,
            IChannelWorldRequestHandler worldRequestHandler, 
            IPlayerRegistry playerRegistry)
            : base(clientFactory)
        {
            this.World = worldRequestHandler;
            this.playerRegistry = playerRegistry;
        }

        /// <inheritdoc />
        public override void Configure(ServiceConfiguration configuration)
        {
            this.channelConfiguration = new ChannelConfiguration(configuration);
            this.SetUp();
        }

        private void SetUp()
        {
            var c = this.channelConfiguration;
            this.ChannelId = c.ChannelId;
            this.WorldId = c.WorldId;
        }

        /// <inheritdoc />
        public void BroadcastToWorld(CharacterKey sourceKey, IEnumerable<CharacterKey> targets, byte[] data)
        {
            var keys = from key in targets
                      where key != sourceKey
                      select key;

            this.BroadcastToWorld(keys, data);
        }

        private void BroadcastToWorld(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // Arrays are more efficient for remote operations.
            CharacterKey[] keys = targets.ToArray();

            var players = from target in this.playerRegistry.Scan(keys)
                          select target.Key;

            this.BroadcastIntoChannel(players, data);
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
            var players = from target in playerRegistry.Scan(targets)
                          select target;

            foreach (var player in players)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
