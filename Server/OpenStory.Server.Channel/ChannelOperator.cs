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
        private ChannelConfiguration _channelConfiguration;

        /// <summary>
        /// Gets the <see cref="IPlayerRegistry"/> for this operator.
        /// </summary>
        public IPlayerRegistry PlayerRegistry { get; }

        /// <inheritdoc />
        public ChannelOperator(IGameClientFactory<ChannelClient> clientFactory, IPlayerRegistry playerRegistry)
            : base(clientFactory)
        {
            PlayerRegistry = playerRegistry;
        }

        /// <inheritdoc />
        public override void Configure(OsServiceConfiguration configuration)
        {
            _channelConfiguration = new ChannelConfiguration(configuration);
            SetUp();
        }

        private void SetUp()
        {
            ChannelId = _channelConfiguration.ChannelId;
        }

        #region IWorldToChannelRequestHandler members

        /// <inheritdoc />
        public int ChannelId { get; private set; }

        /// <inheritdoc />
        int IWorldToChannelRequestHandler.Population => PlayerRegistry.Population;

        /// <inheritdoc/>
        public void BroadcastIntoChannel(IEnumerable<CharacterKey> targets, byte[] data)
        {
            // This method will be part of the service contract.
            var players =
                from target in PlayerRegistry.Scan(targets)
                select target;

            foreach (var player in players)
            {
                player.Client.WritePacket(data);
            }
        }

        #endregion
    }
}
