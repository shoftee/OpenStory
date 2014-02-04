using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Game;
using OpenStory.Framework.Contracts;
using OpenStory.Framework.Model.Common;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.World
{
    /// <summary>
    /// World server class! Handles world server stuff.
    /// </summary>
    internal sealed class WorldServer : RegisteredServiceBase,
        IAuthToWorldRequestHandler,
        IChannelToWorldRequestHandler,
        IConfigurableService
    {
        private readonly IWorldInfoProvider worldInfoProvider;
        private readonly Dictionary<int, IWorldToChannelRequestHandler> channels;

        private WorldConfiguration worldConfiguration;

        private WorldInfo info;
        private ActiveWorld activeWorld;

        /// <inheritdoc />
        public int WorldId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldServer"/> class.
        /// </summary>
        public WorldServer(IWorldInfoProvider worldInfoProvider)
        {
            this.worldInfoProvider = worldInfoProvider;
            this.channels = new Dictionary<int, IWorldToChannelRequestHandler>();
        }

        protected override void OnInitializing(OsServiceConfiguration serviceConfiguration)
        {
            base.OnInitializing(serviceConfiguration);

            this.worldConfiguration = new WorldConfiguration(serviceConfiguration);
        }

        public void Configure(OsServiceConfiguration configuration)
        {
            this.WorldId = this.worldConfiguration.WorldId;

            this.info = this.worldInfoProvider.GetWorldById(this.WorldId);
            this.activeWorld = new ActiveWorld(this.info);
        }

        /// <inheritdoc />
        public void BroadcastFromChannel(int channelId, CharacterKey[] targets, byte[] data)
        {
            var handlers =
                from entry in this.channels
                where entry.Key != channelId
                select entry.Value;

            foreach (var handler in handlers.ToArray())
            {
                handler.BroadcastIntoChannel(targets, data);
            }
        }

        /// <inheritdoc />
        public IWorld GetWorldInfo()
        {
            return this.activeWorld;
        }
    }
}
