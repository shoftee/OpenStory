using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides basic methods for game services.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "GameService", CallbackContract = typeof(IServiceStateChanged))]
    public interface IRegisteredService
    {
        /// <summary>
        /// Initializes the service.
        /// </summary>
        [OperationContract]
        void Initialize(OsServiceConfiguration serviceConfiguration);

        /// <summary>
        /// Starts the service.
        /// </summary>
        [OperationContract]
        void Start();

        /// <summary>
        /// Stops the service.
        /// </summary>
        [OperationContract]
        void Stop();

        /// <summary>
        /// Pings the service, causing it to return its state.
        /// </summary>
        [OperationContract]
        void Ping();
    }
}
