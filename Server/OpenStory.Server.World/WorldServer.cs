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
        INexusToWorldRequestHandler, 
        IConfigurableService
    {
        private readonly IServiceContainer<INexusToWorldRequestHandler> nexus;
        private readonly ChannelContainer channelContainer;
        private readonly IWorldInfoProvider worldInfoProvider;

        private WorldConfiguration worldConfiguration;

        private WorldInfo info;

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
            this.nexus = nexus;
            this.channelContainer = channelContainer;
            this.worldInfoProvider = worldInfoProvider;
        }

        protected override void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
            base.OnInitializing(serviceConfiguration);

            this.worldConfiguration = new WorldConfiguration(serviceConfiguration);
        }

        protected override void OnStarting()
        {
            base.OnStarting();

            this.nexus.Register(this);
        }

        protected override void OnStopping()
        {
            this.nexus.Unregister(this);

            base.OnStopping();
        }

        public void Configure(OsServiceConfiguration configuration)
        {
            this.WorldId = this.worldConfiguration.WorldId;

            this.info = this.worldInfoProvider.GetWorldById(this.WorldId);
        }

        /// <inheritdoc />
        public IWorld GetDetails()
        {
            return new ActiveWorld(this.info);
        }

        /// <inheritdoc />
        public void BroadcastFromChannel(int channelId, CharacterKey[] targets, byte[] data)
        {
            var channels =
                from entry in this.channelContainer
                where entry.ChannelId != channelId
                select entry;

            channels.AsParallel().ForAll(c => c.BroadcastIntoChannel(targets, data));
        }
    }
}
