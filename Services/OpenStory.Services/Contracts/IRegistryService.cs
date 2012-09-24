using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for Game Service Registration
    /// </summary>
    [ServiceContract(Namespace = null, Name = "RegistryService")]
    public interface IRegistryService
    {
        /// <summary>
        /// Attempts to register a service with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration information for the service.</param>
        /// <param name="token">A variable to hold an access token for this registration.</param>
        /// <returns>an instance of <see cref="ServiceOperationResult"/>.</returns>
        ServiceOperationResult TryRegisterService(ServiceConfiguration configuration, out Guid token);

        /// <summary>
        /// Attempts to unregister the service with the specified token.
        /// </summary>
        /// <param name="registrationToken">The registration token issued when the service was registered.</param>
        /// <returns>an instance of <see cref="ServiceOperationResult"/>.</returns>
        ServiceOperationResult TryUnregisterService(Guid registrationToken);
    }
}