using System.ComponentModel;
using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides initialization methods for manager types.
    /// </summary>
    /// <typeparam name="TManagerBase">The type of managers to initialize.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInitializeManagersFacade<TManagerBase> : INestedFacade<IInitializeFacade>, IFluentInterface
        where TManagerBase : ManagerBase
    {
        /// <summary>
        /// Initializes a manager type with a specific manager instance.
        /// </summary>
        /// <typeparam name="TManager">The type to initialize.</typeparam>
        /// <param name="manager">The manager instance.</param>
        /// <returns>the facade object.</returns>
        IInitializeManagersFacade<TManagerBase> Manager<TManager>(TManager manager)
            where TManager : TManagerBase;

        /// <summary>
        /// Initializes the default manager instance for this type.
        /// </summary>
        /// <param name="manager">The manager instance.</param>
        /// <returns>the facade object.</returns>
        IInitializeManagersFacade<TManagerBase> DefaultManager(TManagerBase manager);

        /// <summary>
        /// Initializes a manager type with a specific manager instance and enters the component initialization facade.
        /// </summary>
        /// <typeparam name="TManager">The type to initialize.</typeparam>
        /// <param name="manager">The manager instance.</param>
        /// <returns>a component initialization facade for the provided manager.</returns>
        IInitializeManagerComponentsFacade<TManagerBase> WithComponents<TManager>(TManager manager)
            where TManager : TManagerBase;

        /// <summary>
        /// Initializes the default manager instance for this type and enters the component initialization facade.
        /// </summary>
        /// <param name="manager">The manager instance.</param>
        /// <returns>a component initialization facade for the provided manager.</returns>
        IInitializeManagerComponentsFacade<TManagerBase> DefaultWithComponents(TManagerBase manager);
    }
}
