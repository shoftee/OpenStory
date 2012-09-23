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

    /// <summary>
    /// Represents a base class for nexus fragment implementations.
    /// </summary>
    public abstract class NexusServiceFragmentBase : INexusServiceFragment
    {
        /// <summary>
        /// Gets the nexus service reference.
        /// </summary>
        protected INexusService Service { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="NexusServiceFragmentBase"/>.
        /// </summary>
        /// <param name="uri">The URI of the nexus service.</param>
        protected NexusServiceFragmentBase(Uri uri)
        {
            this.Service = new NexusServiceClient(uri);
        }

        /// <inheritdoc />
        public abstract ServiceState TryGetServiceUri(Guid accessToken, out Uri uri);
    }
}