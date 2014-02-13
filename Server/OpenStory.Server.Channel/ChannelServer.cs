using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a channel server.
    /// </summary>
    public class ChannelServer : GameServer<ChannelOperator>
    {
        private readonly IServiceContainer<IWorldToChannelRequestHandler> world;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelServer"/> class.
        /// </summary>
        /// <param name="process"><inheritdoc /></param>
        /// <param name="channelOperator">The <see cref="ChannelOperator"/> to use for this server.</param>
        /// <param name="world">The channel container.</param>
        public ChannelServer(IServerProcess process, ChannelOperator channelOperator, IServiceContainer<IWorldToChannelRequestHandler> world)
            : base(process, channelOperator)
        {
            this.world = world;
        }

        /// <inheritdoc />
        protected override void OnStarting()
        {
            base.OnStarting();

            this.world.Register(this.Operator);
        }

        /// <inheritdoc />
        protected override void OnStopping()
        {
            this.world.Unregister(this.Operator);

            base.OnStopping();
        }
    }
}
