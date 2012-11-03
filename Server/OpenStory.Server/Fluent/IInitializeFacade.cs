using System.ComponentModel;
using OpenStory.Server.Modules;
using OpenStory.Server.Modules.Logging;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for initializing OpenStory managers.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInitializeFacade : IFluentInterface
    {
        /// <summary>
        /// The entry point to a <typeparamref name="TManager"/> initialization facade.
        /// </summary>
        /// <typeparam name="TManager">The type of the managers to initialize.</typeparam>
        IInitializeManagersFacade<TManager> Managers<TManager>()
            where TManager : ManagerBase;

        /// <summary>
        /// Initializes a single manager as the default for the manager type.
        /// </summary>
        /// <typeparam name="TManager">The manager type to initialize.</typeparam>
        /// <param name="instance">The instance to use for the default manager.</param>
        /// <returns>the root initialization facade.</returns>
        IInitializeFacade Manager<TManager>(TManager instance)
            where TManager : ManagerBase;

        /// <summary>
        /// Initializes the <see cref="ILogger"/> component of the default <see cref="LogManager"/>.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> instance to use.</param>
        /// <returns>the root initialization facade.</returns>
        IInitializeFacade Logger(ILogger logger);

        /// <summary>
        /// The entry point for the service initialization facade.
        /// </summary>
        IInitializeServiceFacade Services();
    }
}
