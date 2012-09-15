using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides service discovery for game services.
    /// </summary>
    [ServiceContract(Namespace = null, Name = null)]
    public interface INexusService
    {
        /// <summary>
        /// Retrieves the endpoint address for the account service.
        /// </summary>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetAccountServiceUri(out Uri uri);

        /// <summary>
        /// Retrieves the endpoint address for the account service.
        /// </summary>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetAuthService(out Uri uri);

        /// <summary>
        /// Retrieves the endpoint address for the authentication service.
        /// </summary>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetChannelService(out Uri uri);

        /// <summary>
        /// Retrieves the endpoint address for the account service.
        /// </summary>
        /// <param name="uri">A variable to hold the address.</param>
        /// <returns>
        /// the <see cref="ServiceState"/> of the service if it was discovered successfully;
        /// otherwise, <see cref="ServiceState.Unknown"/>
        /// </returns>
        ServiceState TryGetWorldService(out Uri uri);
    }
}
