using System;
using System.ServiceModel.Description;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides a method for getting a service endpoint by an URI.
    /// </summary>
    public interface IEndpointProvider
    {
        /// <summary>
        /// Gets the endpoint to the service at the specified URI.
        /// </summary>
        /// <param name="serviceType">The service interface type.</param>
        /// <param name="uri">The URI to the service.</param>
        /// <returns>an instance of <see cref="ServiceEndpoint"/>.</returns>
        ServiceEndpoint GetEndpoint(Type serviceType, Uri uri);
    }
}