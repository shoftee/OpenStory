using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides basic methods for game services.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "GameService", CallbackContract = typeof(IServiceStateChanged))]
    public interface IManagedService
    {
        /// <summary>
        /// Initializes the service.
        /// </summary>
        [OperationContract]
        ServiceOperationResult Initialize();

        /// <summary>
        /// Starts the service.
        /// </summary>
        [OperationContract]
        ServiceOperationResult Start();

        /// <summary>
        /// Stops the service.
        /// </summary>
        [OperationContract]
        ServiceOperationResult Stop();

        /// <summary>
        /// Gets the service state.
        /// </summary>
        [OperationContract]
        ServiceOperationResult GetServiceState();
    }
}
