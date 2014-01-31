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
    public sealed class ChannelOperator : ServerOperator<ChannelClient>, IWorldToChannelRequestHandler
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
        public int Population
        {
            get { return this.playerRegistry.Population; }
        }

        /// <inheritdoc />
        public ChannelOperator(IGameClientFactory<ChannelClient> clientFactory, IPlayerRegistry playerRegistry)
            : base(clientFactory)
        {
            this.playerRegistry = playerRegistry;
        }

        /// <inheritdoc />
        public override void Configure(OsServiceConfiguration configuration)
        {
            this.channelConfiguration = new ChannelConfiguration(configuration);
            this.SetUp();
        }

        private void SetUp()
        {
            this.ChannelId = this.channelConfiguration.ChannelId;
            this.WorldId = this.channelConfiguration.WorldId;
        }

        /// <inheritdoc/>
        public void BroadcastIntoChannel(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // This method will be part of the service contract.
            var players =
                from target in this.playerRegistry.Scan(targets)
                select target;

            foreach (var player in players)
            {
                player.Client.WritePacket(data);
            }
        }
    }
}
