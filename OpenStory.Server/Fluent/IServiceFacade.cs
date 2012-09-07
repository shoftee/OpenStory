using System.ComponentModel;

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
        /// The entry point for managing a specific Channel service.
        /// </summary>
        /// <param name="id">The identifier of the channel.</param>
        IChannelServiceFacade Channel(int id);

        /// <summary>
        /// The entry point for managing a specific Channel service.
        /// </summary>
        /// <param name="worldId">The identifier of the host world.</param>
        /// <param name="channelId">The identifier of the channel.</param>
        IChannelServiceFacade Channel(int worldId, int channelId);

        /// <summary>
        /// The entry point for managing the World services.
        /// </summary>
        IWorldsServiceFacade Worlds();

        /// <summary>
        /// The entry point for managing a specific World service.
        /// </summary>
        /// <param name="id">The identifier of the world.</param>
        IWorldServiceFacade World(int id);

    }
}