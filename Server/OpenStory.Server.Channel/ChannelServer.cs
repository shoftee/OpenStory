using OpenStory.Server.Processing;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Channel
{
    /// <summary>
    /// Represents a channel server.
    /// </summary>
    public class ChannelServer : NetworkServer<ChannelOperator>
    {
        private readonly IServiceContainer<IWorldToChannelRequestHandler> world;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelServer"/> class.
        /// </summary>
        public ChannelServer(IServerProcess process, ChannelOperator @operator, IServiceContainer<IWorldToChannelRequestHandler> world)
            : base(process, @operator)
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
