using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for creating <see cref="ServiceHost"/> instances.
    /// </summary>
    public interface IServiceHostFactory
    {
        /// <summary>
        /// Creates a service host for a service.
        /// </summary>
        /// <returns>the created instance.</returns>
        ServiceHost CreateServiceHost();
    }
}