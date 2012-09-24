using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides service discovery for game services.
    /// </summary>
    [ServiceContract(Namespace = null, Name = "NexusService")]
    public interface INexusService
    {
        /// <summary>
        /// Retrieves the service configuration data for a service.
        /// </summary>
        /// <param name="accessToken">The access token key for the service to request the data for.</param>
        /// <param name="configuration">A variable to hold the configuration.</param>
        /// <returns>an instance of <see cref="ServiceOperationResult"/>.</returns>
        ServiceOperationResult TryGetServiceConfiguration(Guid accessToken, out ServiceConfiguration configuration);
    }
}
