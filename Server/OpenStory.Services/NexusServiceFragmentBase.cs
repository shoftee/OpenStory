using System;
using OpenStory.Services.Clients;
using OpenStory.Services.Contracts;

namespace OpenStory.Services
{
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