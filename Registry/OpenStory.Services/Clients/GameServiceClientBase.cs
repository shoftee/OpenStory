using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Discovery;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a base class for game service clients.
    /// </summary>
    public abstract class GameServiceClientBase<TServiceInterface> : ClientBase<TServiceInterface>
        where TServiceInterface : class
    {
        /// <summary>
        /// Initialized a new instance of the <see cref="GameServiceClientBase{TServiceInterface}"/> class with the specified endpoint address.
        /// </summary>
        /// <param name="endpoint">The endpoint information for the service.</param>
        protected GameServiceClientBase(ServiceEndpoint endpoint)
            : base(endpoint)
        {
        }
    }
}