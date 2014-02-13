using System.Collections.Generic;
using System.Linq;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a server that handles the channel-side operations.
    /// </summary>
    public sealed class ChannelOperator : ServerOperator<ChannelClient>, IWorldToChannelRequestHandler
    {
        private ChannelConfiguration channelConfiguration;

        /// <summary>
        /// Gets the <see cref="IPlayerRegistry"/> for this operator.
        /// </summary>
        public IPlayerRegistry PlayerRegistry { get; private set; }

        /// <inheritdoc />
        public ChannelOperator(
            IGameClientFactory<ChannelClient> clientFactory, 
            IPlayerRegistry playerRegistry)
            : base(clientFactory)
        {
            this.PlayerRegistry = playerRegistry;
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
        }

        #region IWorldToChannelRequestHandler members

        /// <inheritdoc />
        public int ChannelId { get; private set; }

        /// <inheritdoc />
        int IWorldToChannelRequestHandler.Population
        {
            get { return this.PlayerRegistry.Population; }
        }

        /// <inheritdoc/>
        public void BroadcastIntoChannel(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // This method will be part of the service contract.
            var players =
                from target in this.PlayerRegistry.Scan(targets)
                select target;

            foreach (var player in players)
            {
                player.Client.WritePacket(data);
            }
        }
        
        #endregion
    }
}
