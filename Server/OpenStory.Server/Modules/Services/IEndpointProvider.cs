using System;
using System.ServiceModel.Description;

namespace OpenStory.Server.Modules.Services
{
    /// <summary>
    /// Provides a method for getting a service endpoint by an URI.
    /// </summary>
    public interface IEndpointProvider
    {
        /// <summary>
        /// Gets the endpoint to the service at the specified URI.
        /// </summary>
        /// <typeparam name="TServiceInterface">The service interface type.</typeparam>
        /// <param name="uri">The URI to the service.</param>
        /// <returns>an instance of <see cref="ServiceEndpoint"/>.</returns>
        ServiceEndpoint GetEndpoint<TServiceInterface>(Uri uri) where TServiceInterface : class;
    }
}