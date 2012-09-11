using System.ComponentModel;
using OpenStory.Server.Data;
using OpenStory.Server.Diagnostics;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for initializing OpenStory managers.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInitializeFacade : IFluentInterface
    {
        /// <summary>
        /// The entry point to the <see cref="DataManager"/> initialization facade.
        /// </summary>
        IInitializeManagersFacade<DataManager> DataManagers();

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
