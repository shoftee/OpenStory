namespace OpenStory.Server.Fluent.Service
{
    internal sealed class ServiceFacade : IServiceFacade
    {
        /// <inheritdoc />
        public IAccountServiceFacade Accounts()
        {
            return new AccountServiceFacade();
        }

        /// <inheritdoc />
        public IAuthServiceFacade Auth()
        {
            return new AuthServiceFacade();
        }

        /// <inheritdoc />
        public IChannelsServiceFacade Channels()
        {
            return new ChannelsServiceFacade();
        }

        /// <inheritdoc />
        public IChannelServiceFacade Channel()
        {
            return new ChannelServiceFacade();
        }

        /// <inheritdoc />
        public IChannelServiceFacade Channel(int id)
        {
            return new ChannelServiceFacade(id);
        }

        /// <inheritdoc />
        public IChannelServiceFacade Channel(int worldId, int channelId)
        {
            return new ChannelServiceFacade(worldId, channelId);
        }

        /// <inheritdoc />
        public IWorldsServiceFacade Worlds()
        {
            return new WorldsServiceFacade();
        }

        /// <inheritdoc />
        public IWorldServiceFacade World()
        {
            return new WorldServiceFacade();
        }

        /// <inheritdoc />
        public IWorldServiceFacade World(int id)
        {
            return new WorldServiceFacade(id);
        }
    }
}