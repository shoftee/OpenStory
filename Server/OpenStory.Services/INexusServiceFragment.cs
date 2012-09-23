using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
    /// <summary>
    /// Provides a service-specific interface to the nexus service.
    /// </summary>
    public interface INexusServiceFragment
    {
        /// <summary>
        /// Gets the service URI from the Nexus service.
        /// </summary>
        /// <param name="accessToken">The access token for the service.</param>
        /// <param name="uri">A variable to hold the URI.</param>
        /// <returns>The state of the queried service.</returns>
        ServiceState TryGetServiceUri(Guid accessToken, out Uri uri);
    }
}