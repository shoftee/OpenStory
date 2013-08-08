using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for creating service instances.
    /// </summary>
    public interface IGenericServiceFactory
    {
        /// <summary>
        /// Creates a service host for a service.
        /// </summary>
        /// <returns>the created instance.</returns>
        ServiceHost CreateServiceHost();
    }
}