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
        /// Retrieves the endpoint address for the account service.
        /// </summary>
        /// <param name="accessToken">The registration token for the calling service.</param>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetAccountServiceUri(Guid accessToken, out Uri uri);

        /// <summary>
        /// Retrieves the endpoint address for the account service.
        /// </summary>
        /// <param name="accessToken">The registration token for the calling service.</param>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetAuthServiceUri(Guid accessToken, out Uri uri);

        /// <summary>
        /// Retrieves the endpoint address for the authentication service.
        /// </summary>
        /// <param name="accessToken">The registration token for the calling service.</param>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetChannelServiceUri(Guid accessToken, out Uri uri);

        /// <summary>
        /// Retrieves the endpoint address for the account service.
        /// </summary>
        /// <param name="accessToken">The registration token for the calling service.</param>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetWorldServiceUri(Guid accessToken, out Uri uri);
    }
}
