using OpenStory.Server.Modules.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class ServiceFacade : IServiceFacade
    {
        /// <inheritdoc />
        public IAccountServiceFacade Accounts()
        {
            return new AccountServiceFacade(this);
        }

        /// <inheritdoc />
        public IAuthServiceFacade Auth()
        {
            return new AuthServiceFacade(this);
        }

        /// <inheritdoc />
        public IChannelsServiceFacade Channels()
        {
            return new ChannelsServiceFacade(this);
        }

        /// <inheritdoc />
        public IChannelServiceFacade Channel()
        {
            return new ChannelServiceFacade(this);
        }

        /// <inheritdoc />
        public IWorldServiceFacade World()
        {
            return new WorldServiceFacade(this);
        }

        /// <inheritdoc />
        public IGameService Local()
        {
            return ServiceManager.GetManager().LocalAs<IGameService>();
        }

        /// <inheritdoc />
        public TGameService LocalAs<TGameService>()
            where TGameService : class, IGameService
        {
            return ServiceManager.GetManager().LocalAs<TGameService>();
        }
    }
}