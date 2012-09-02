namespace OpenStory.Server.Modules
{
    /// <summary>
    /// Represents a strongly-typed manager class, which allows manager registration.
    /// </summary>
    /// <typeparam name="TManagerBase">The type of managers that this class will handle.</typeparam>
    public abstract class ManagerBase<TManagerBase> : ManagerBase
        where TManagerBase : ManagerBase
    {
        private static readonly ManagerStore<TManagerBase> ManagerStore =
            new ManagerStore<TManagerBase>();

        /// <summary>
        /// Registers the default manager instance for this manager type.
        /// </summary>
        /// <param name="manager">The default manager for this manager type.</param>
        public static void RegisterDefault(TManagerBase manager)
        {
            ManagerStore.RegisterDefault(manager);
        }

        /// <summary>
        /// Registers a type-specific manager for this manager type.
        /// </summary>
        /// <typeparam name="TManager">The type of the registered manager.</typeparam>
        /// <param name="manager">The instance to register.</param>
        public static void RegisterManager<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            ManagerStore.RegisterManager(manager);
        }

        /// <summary>
        /// Retrieves the default manager instance for this manager type.
        /// </summary>
        /// <returns>the default manager instance.</returns>
        public static TManagerBase GetManager()
        {
            return ManagerStore.GetManager<TManagerBase>();
        }

        /// <summary>
        /// Retrieves a type-specific manager for this manager type.
        /// </summary>
        /// <typeparam name="TManager">The type of the manager to retrieve.</typeparam>
        /// <returns>the manager instance, or <c>null</c> if there is no registered instance of this type.</returns>
        public static TManager GetManager<TManager>()
            where TManager : TManagerBase
        {
            return ManagerStore.GetManager<TManager>();
        }
    }
}
