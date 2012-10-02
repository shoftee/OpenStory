using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <summary>
    /// Provides methods for Game Service Registration
    /// </summary>
    [ServiceContract(Namespace = null, Name = "RegistryService")]
    public interface IRegistryService : INexusService
    {
        /// <summary>
        /// Attempts to register a service with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration information for the service.</param>
        /// <param name="token">A variable to hold an access token for this registration.</param>
        /// <returns>an instance of <see cref="OpenStory.Services.Contracts.ServiceOperationResult"/>.</returns>
        ServiceOperationResult TryRegisterService(ServiceConfiguration configuration, out Guid token);

        /// <summary>
        /// Attempts to unregister the service with the specified token.
        /// </summary>
        /// <param name="registrationToken">The registration token issued when the service was registered.</param>
        /// <returns>an instance of <see cref="OpenStory.Services.Contracts.ServiceOperationResult"/>.</returns>
        ServiceOperationResult TryUnregisterService(Guid registrationToken);

        /// <summary>
        /// Attempts to retrieve all registered tokens.
        /// </summary>
        /// <param name="tokens">A variable to hold the token array.</param>
        /// <returns>an instance of <see cref="OpenStory.Services.Contracts.ServiceOperationResult"/>.</returns>
        ServiceOperationResult TryGetRegistrations(out Guid[] tokens);
    }
}