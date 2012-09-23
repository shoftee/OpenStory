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
        /// Registers the "Local" component of the ServiceManager.
        /// </summary>
        /// <param name="local">The service reference to register.</param>
        IInitializeServiceFacade Host<TGameService>(TGameService local)
            where TGameService : class, IGameService;

        /// <summary>
        /// Denotes the nexus service reference to use for configuration and discovery.
        /// </summary>
        /// <param name="nexus">The nexus service reference.</param>
        IInitializeServiceFacade Through(INexusService nexus);

        /// <summary>
        /// Provides the access token required to register the service.
        /// </summary>
        /// <param name="token">The access token.</param>
        INestedFacade<IInitializeFacade> WithAccessToken(Guid token);
    }
}