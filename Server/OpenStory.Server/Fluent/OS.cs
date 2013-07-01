using Ninject;
using OpenStory.Server.Fluent.Config;
using OpenStory.Server.Fluent.Extensions;
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
        /// Gets a service instance for the provided type.
        /// </summary>
        /// <typeparam name="TService">The type of the service to get an instance of.</typeparam>
        /// <returns>the service instance.</returns>
        public static TService Get<TService>()
        {
            return ninject.TryGet<TService>();
        }

        /// <summary>
        /// Gets a service instance for the provided type.
        /// </summary>
        /// <typeparam name="TService">The type of the service to get an instance of.</typeparam>
        /// <param name="name">The name of the instance. </param>
        /// <returns>the service instance.</returns>
        public static TService Get<TService>(string name)
        {
            return ninject.TryGet<TService>(name);
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
