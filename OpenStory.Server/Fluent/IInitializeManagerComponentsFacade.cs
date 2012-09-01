using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides initialization methods for manager components.
    /// </summary>
    /// <typeparam name="TManagerBase">The type of managers that are being initialized.</typeparam>
    public interface IInitializeManagerComponentsFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        /// <summary>
        /// Initializes a component of the manager.
        /// </summary>
        /// <param name="name">The name of the component.</param>
        /// <param name="instance">The instance to use for the component.</param>
        /// <returns>the component initialization facade.</returns>
        IInitializeManagerComponentsFacade<TManagerBase> Component(string name, object instance);

        /// <summary>
        /// Returns to the manager initialization facade.
        /// </summary>
        /// <returns>the parent facade.</returns>
        IInitializeManagerFacade<TManagerBase> Done();
    }
}