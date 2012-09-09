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
        /// <param name="token">A variable to hold an access token for this registration.</param>
        /// <returns>The current state of the registered service.</returns>
        ServiceState TryRegisterAuthService(Uri uri, out Guid token);

        /// <summary>
        /// Attempts to register an account service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <param name="token">A variable to hold an access token for this registration.</param>
        /// <returns><c>true</c> if the service was discovered successfully; otherwise, <c>false</c>.</returns>
        ServiceState TryRegisterAccountService(Uri uri, out Guid token);

        /// <summary>
        /// Attempts to register a world service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <param name="token">A variable to hold an access token for this registration.</param>
        /// <returns><c>true</c> if the service was discovered successfully; otherwise, <c>false</c>.</returns>
        ServiceState TryRegisterWorldService(Uri uri, int worldId, out Guid token);

        /// <summary>
        /// Attempts to register a channel service module.
        /// </summary>
        /// <param name="uri">The URI for the service.</param>
        /// <param name="worldId">The public world identifier for the service instance.</param>
        /// <param name="channelId">The public channel identifier for the channel instance.</param>
        /// <param name="token">A variable to hold an access token for this registration.</param>
        /// <returns><c>true</c> if the service was discovered successfully; otherwise, <c>false</c>.</returns>
        ServiceState TryRegisterChannelService(Uri uri, int worldId, int channelId, out Guid token);

        /// <summary>
        /// Attempts to unregister the service with the specified token.
        /// </summary>
        /// <param name="registrationToken">The registration token issued when the service was registered.</param>
        /// <returns><c>true</c> if the service was unregistered successfully; otherwise, <c>false</c>.</returns>
        ServiceState TryUnregisterService(Guid registrationToken);
    }
}
