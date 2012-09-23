using System;
using System.ComponentModel;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for initializing 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInitializeServiceFacade : INestedFacade<IInitializeFacade>
    {
        /// <summary>
        /// Registers the "LocalService" component of the ServiceManager.
        /// </summary>
        /// <param name="local">The service reference to register.</param>
        /// <param name="nexusFragment">The nexus interface to use specifically for the newly hosted service.</param>
        IInitializeServiceFacade Host(IManagedService local, INexusServiceFragment nexusFragment);

        /// <summary>
        /// Provides the access token required to register the service.
        /// </summary>
        /// <param name="token">The access token.</param>
        INestedFacade<IInitializeFacade> WithAccessToken(Guid token);
    }
}