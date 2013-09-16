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
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        Guid RegisterService(ServiceConfiguration configuration);

        /// <summary>
        /// Attempts to unregister the service with the specified token.
        /// </summary>
        /// <param name="token">The registration token issued when the service was registered.</param>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        void UnregisterService(Guid token);

        /// <summary>
        /// Attempts to retrieve all registered tokens.
        /// </summary>
        /// <returns>the result of the operation.</returns>
        [OperationContract]
        Guid[] GetRegistrations();
    }
}