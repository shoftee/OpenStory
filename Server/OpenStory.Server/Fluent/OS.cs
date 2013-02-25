using Ninject;
using OpenStory.Server.Data;
using OpenStory.Server.Fluent.Config;
using OpenStory.Server.Fluent.Extensions;
using OpenStory.Server.Fluent.Initialize;
using OpenStory.Server.Fluent.Lookup;
using OpenStory.Server.Fluent.Service;
using OpenStory.Server.Modules.Logging;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// The OpenStory management entry point.
    /// </summary>
    public static class OS
    {
        private static IKernel ninject;

        /// <summary>
        /// The entry point for the configuration fluent interface.
        /// </summary>
        /// <returns></returns>
        public static IConfigFacade Config()
        {
            return new ConfigFacade();
        }

        /// <summary>
        /// The entry point for the initialization fluent interface.
        /// </summary>
        public static void Initialize(IKernel kernel)
        {
            ninject = kernel;
        }
        
        /// <summary>
        /// Retrieves the default <see cref="ILogger"/> instance.
        /// </summary>
        /// <returns>an instance of <see cref="ILogger"/>.</returns>
        public static ILogger Log()
        {
            return ninject.Get<ILogger>();
        }
        
        /// <summary>
        /// The entry point for the lookup fluent interface.
        /// </summary>
        public static ILookupFacade Lookup()
        {
            return ninject.Get<ILookupFacade>();
        }

        /// <summary>
        /// The entry point for the service fluent interface.
        /// </summary>
        public static IServiceFacade Svc()
        {
            return ninject.Get<IServiceFacade>();
        }

        /// <summary>
        /// Retrieves the <see cref="DataManager"/> registered for the provided type.
        /// </summary>
        /// <typeparam name="TDataManager">The type to get the manager of.</typeparam>
        /// <returns>an instance of <tupeparamref name="TDataManager" />, or <c>null</c> if no manager was found.</returns>
        public static TDataManager Data<TDataManager>()
            where TDataManager : DataManager
        {
            return DataManager.GetManager<TDataManager>();
        }

        /// <summary>
        /// Retrieves the default <see cref="DataManager"/>.
        /// </summary>
        /// <returns>an instance of <see cref="DataManager"/>, or <c>null</c> if there was no registered default.</returns>
        public static DataManager Data()
        {
            return DataManager.GetManager();
        }

        #region Extensions
        
        private static readonly IFluentOsExtensions OsEx = new FluentOsExtensions();

        /// <summary>
        /// The entry point for fluent interface extensions.
        /// </summary>
        public static IFluentOsExtensions Ex()
        {
            return OsEx;
        }

        #endregion
    }
}
