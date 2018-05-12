using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    /// <summary>
    /// World server class! Handles world server stuff.
    /// </summary>
    internal sealed class WorldServer : GameServerBase, 
        IChannelToWorldRequestHandler, 
        INexusToWorldRequestHandler
    {
        private readonly IServiceContainer<INexusToWorldRequestHandler> _nexus;
        private readonly ChannelContainer _channelContainer;
        private readonly IWorldInfoProvider _worldInfoProvider;

        private WorldConfiguration _worldConfiguration;

        private WorldInfo _info;

        /// <inheritdoc />
        public int WorldId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldServer"/> class.
        /// </summary>
        public WorldServer(
            IServiceContainer<INexusToWorldRequestHandler> nexus, 
            ChannelContainer channelContainer,
            IWorldInfoProvider worldInfoProvider)
        {
            _nexus = nexus;
            _channelContainer = channelContainer;
            _worldInfoProvider = worldInfoProvider;
        }

        protected override void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
            base.OnInitializing(serviceConfiguration);

            _worldConfiguration = new WorldConfiguration(serviceConfiguration);

            WorldId = _worldConfiguration.WorldId;
            _info = _worldInfoProvider.GetWorldById(WorldId);
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            _nexus.Register(this);
        }

        protected override void OnStopping()
        {
            _nexus.Unregister(this);

            base.OnStopping();
        }

        /// <inheritdoc />
        public IWorld GetDetails()
        {
            return new ActiveWorld(_info);
        }

        /// <inheritdoc />
        public void BroadcastFromChannel(int channelId, CharacterKey[] targets, byte[] data)
        {
            var channels =
                from entry in _channelContainer
                where entry.ChannelId != channelId
                select entry;

            channels.AsParallel().ForAll(c => c.BroadcastIntoChannel(targets, data));
        }
    }
}
