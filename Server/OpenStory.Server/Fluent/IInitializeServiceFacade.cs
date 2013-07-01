using System.ComponentModel;
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
        /// <typeparam name="TGameService">The service type derived from <see cref="IGameService"/>.</typeparam>
        /// <param name="local">The service reference to register.</param>
        IInitializeServiceFacade Host<TGameService>(TGameService local)
            where TGameService : class, IGameService;
    }
}