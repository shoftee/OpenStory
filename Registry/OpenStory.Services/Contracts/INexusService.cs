using System;
using System.Collections.Generic;
using System.ServiceModel;
using OpenStory.Framework.Contracts;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides service discovery for game services.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "NexusService")]
    public interface INexusService
    {
        /// <summary>
        /// Attempts to retrieve the service configuration data for a service.
        /// </summary>
        /// <param name="token">An access token for the nexus operation.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(Guid token);
    }

    /// <summary>
    /// Provides methods for querying a nexus service for channel service information.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "NexusChannelService")]
    public interface INexusChannelService
    {
        /// <summary>
        /// Attempts to retrieve the URI of a channel service.
        /// </summary>
        /// <param name="token">An access token for the nexus operation.</param>
        /// <param name="channelId">The world-scoped identifier of the channel instance.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<Uri> GetChannelUri(Guid token, int channelId);
    }

    /// <summary>
    /// Provides methods for querying a nexus service for world service information.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "NexusWorldService")]
    public interface INexusWorldService
    {
        /// <summary>
        /// Attempts to retrieve the URI of the world service.
        /// </summary>
        /// <param name="token">An access token for the nexus operation.</param>
        /// <param name="worldId">The identifier of the world instnace.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<Uri> GetWorldUri(Guid token, int worldId);

        /// <summary>
        /// Attempts to retrieve the list of channel load values for a world.
        /// </summary>
        /// <param name="token">An access token for the nexus operation.</param>
        /// <param name="worldId">The world to query.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<Dictionary<int, Uri>> GetChannelUris(Guid token, int worldId);
    }

    /// <summary>
    /// Provides methods for querying a nexus service for authentication service information.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "NexusAuthService")]
    public interface INexusAuthService
    {
        /// <summary>
        /// Attempts to retrieve the URI of the auth service.
        /// </summary>
        /// <param name="token">An access token for the nexus operation.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<Uri> GetAuthUri(Guid token);
    }
}
