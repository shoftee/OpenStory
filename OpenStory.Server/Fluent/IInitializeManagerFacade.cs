using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides initialization methods for manager types.
    /// </summary>
    /// <typeparam name="TManagerBase">The type of managers to initialize.</typeparam>
    public interface IInitializeManagerFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        /// <summary>
        /// Initializes a manager type with a specific manager instance.
        /// </summary>
        /// <typeparam name="TManager">The type to initialize.</typeparam>
        /// <param name="manager">The manager instance.</param>
        /// <returns>the facade object.</returns>
        IInitializeManagerFacade<TManagerBase> With<TManager>(TManager manager)
            where TManager : TManagerBase;

        /// <summary>
        /// Initializes the default manager instance for this type.
        /// </summary>
        /// <param name="manager">The manager instance.</param>
        /// <returns>the parent facade object.</returns>
        IInitializeFacade WithDefault(TManagerBase manager);
    }
}