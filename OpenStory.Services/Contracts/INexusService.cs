using System;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for Game Service Registration
    /// </summary>
    public interface INexusService : IGameService
    {
        /// <summary>
        /// Attempts to register an authentication service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <returns>if the service connects successfully, an access token for the service instance; otherwise, <c>null</c>.</returns>
        string TryRegisterAuthService(Uri uri);

        /// <summary>
        /// Attempts to unregister the authentication service module.
        /// </summary>
        /// <param name="accessToken">The access token that corresponds to the active service.</param>
        /// <returns><c>true</c> if the service was unregistered successfully; otherwise, <c>false</c>.</returns>
        bool TryUnregisterAuthService(string accessToken);

        /// <summary>
        /// Attempts to register an account service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <returns>if the service connects successfully, an access token for the service instance; otherwise, <c>null</c>.</returns>
        string TryRegisterAccountService(Uri uri);

        /// <summary>
        /// Attempts to unregister the account service module.
        /// </summary>
        /// <param name="accessToken">The access token that corresponds to the active service.</param>
        /// <returns><c>true</c> if the service was unregistered successfully; otherwise, <c>false</c>.</returns>
        bool TryUnregisterAccountService(string accessToken);

        /// <summary>
        /// Attempts to register a world service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <returns>if the service connects successfully, an access token for the service instance; otherwise, <c>null</c>.</returns>
        string TryRegisterWorldService(Uri uri, int worldId);

        /// <summary>
        /// Attempts to unregister a world service module.
        /// </summary>
        /// <param name="accessToken">The access token that corresponds to the active service.</param>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <returns><c>true</c> if the service was unregistered successfully; otherwise, <c>false</c>.</returns>
        bool TryUnregisterWorldService(string accessToken, int worldId);

        /// <summary>
        /// Attempts to register a channel service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <param name="channelId">The public channel identifier for the channel instance.</param>
        /// <returns>if the service connects successfully, an access token for the service instance; otherwise, <c>null</c>.</returns>
        string TryRegisterChannelService(Uri uri, int worldId, int channelId);

        /// <summary>
        /// Attempts to unregister a channel service module.
        /// </summary>
        /// <param name="accessToken">The access token that corresponds to the active service.</param>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <param name="channelId">The public channel identifier for the channel instance.</param>
        /// <returns><c>true</c> if the service was unregistered successfully; otherwise, <c>false</c>.</returns>
        bool TryUnregisterChannelService(string accessToken, int worldId, int channelId);

        /// <summary>
        /// Gets a binding to the authentication service.
        /// </summary>
        /// <returns>a <see cref="IAuthService"/> object.</returns>
        IAuthService GetAuthService();

        /// <summary>
        /// Gets a binding to the account service.
        /// </summary>
        /// <returns>a <see cref="IAccountService"/> object.</returns>
        IAccountService GetAccountService();

        /// <summary>
        /// Gets a binding to a world service.
        /// </summary>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <returns>a <see cref="IWorldService"/> object.</returns>
        IWorldService GetWorldService(int worldId);

        /// <summary>
        /// Gets a binding to a channel service.
        /// </summary>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <param name="channelId">The public channel identifier for the channel instance.</param>
        /// <returns>a <see cref="IChannelService"/> object.</returns>
        IChannelService GetChannelService(int worldId, int channelId);
    }
}
