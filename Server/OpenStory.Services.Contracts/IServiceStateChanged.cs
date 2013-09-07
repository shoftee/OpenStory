using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides the callback interface for service state changes.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "ServiceStateChangedCallback")]
    public interface IServiceStateChanged
    {
        /// <summary>
        /// Called when the state of the service changes.
        /// </summary>
        /// <param name="oldState">The state of the service before the change.</param>
        /// <param name="newState">The state of the service after the change.</param>
        void OnServiceStateChanged(ServiceState oldState, ServiceState newState);
    }
}
