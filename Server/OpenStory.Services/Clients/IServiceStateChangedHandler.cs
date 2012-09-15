using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// The interface for handling service state changes.
    /// </summary>
    internal interface IServiceStateChangedHandler
    {
        void OnServiceStateChanged(ServiceState newState);
    }
}