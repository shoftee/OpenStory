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
        /// Attempts to retrieve the service configuration data for a service.
        /// </summary>
        /// <param name="token">An access token for the nexus operation.</param>
        [OperationContract]
        [FaultContract(typeof(InvalidOperationException))]
        Uri GetServiceUri(Guid token);
    }
}
