using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides basic methods for game services.
    /// </summary>
    [ServiceContract(Namespace = null, CallbackContract = typeof(IServiceStateChanged))]
    public interface IGameService
    {
        /// <summary>
        /// Initializes the service.
        /// </summary>
        [OperationContract]
        ServiceState Initialize();

        /// <summary>
        /// Starts the service.
        /// </summary>
        [OperationContract]
        ServiceState Start();

        /// <summary>
        /// Stops the service.
        /// </summary>
        [OperationContract]
        ServiceState Stop();

        /// <summary>
        /// Gets the service state.
        /// </summary>
        [OperationContract]
        ServiceState GetServiceState();
    }
}
