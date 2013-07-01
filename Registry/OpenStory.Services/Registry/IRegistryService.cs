using System;
using System.ServiceModel;
using OpenStory.Framework.Contracts;
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
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<Guid> RegisterService(ServiceConfiguration configuration);

        /// <summary>
        /// Attempts to unregister the service with the specified token.
        /// </summary>
        /// <param name="token">The registration token issued when the service was registered.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult UnregisterService(Guid token);

        /// <summary>
        /// Attempts to retrieve all registered tokens.
        /// </summary>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        ServiceOperationResult<Guid[]> GetRegistrations();
    }
}