using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing the OpenStory services.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceFacade : IFluentInterface
    {
        /// <summary>
        /// The entry point for managing the Account service.
        /// </summary>
        IAccountServiceFacade Accounts();

        /// <summary>
        /// The entry point for managing the Authentication service.
        /// </summary>
        IAuthServiceFacade Auth();

        /// <summary>
        /// The entry point for managing the Channel services.
        /// </summary>
        IChannelsServiceFacade Channels();

        /// <summary>
        /// The entry point for managing the current Channel service.
        /// </summary>
        IChannelServiceFacade Channel();

        /// <summary>
        /// The entry point for managing the current World service.
        /// </summary>
        IWorldServiceFacade World();

        /// <summary>
        /// Gets the service registered as local.
        /// </summary>
        IGameService Local();
    }
}