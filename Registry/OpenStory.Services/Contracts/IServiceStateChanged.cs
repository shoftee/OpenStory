using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// The callback interface for service state changes.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "ServiceStateChangedCallback")]
    public interface IServiceStateChanged
    {
        /// <summary>
        /// The method called when the state of the service changes.
        /// </summary>
        /// <param name="newState"></param>
        [OperationContract(IsOneWay = true)]
        void OnServiceStateChanged(ServiceState newState);
    }
}
