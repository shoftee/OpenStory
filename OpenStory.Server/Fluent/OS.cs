using OpenStory.Server.Data;
using OpenStory.Server.Diagnostics;
using OpenStory.Server.Fluent.Internal;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// The OpenStory management entry point.
    /// </summary>
    public static class OS
    {
        /// <summary>
        /// The entry point for the initialization fluent API.
        /// </summary>
        /// <returns>an instance of type <see cref="IInitializeFacade"/>.</returns>
        public static IInitializeFacade Initialize()
        {
            return new InitializeFacade();
        }

        /// <summary>
        /// The entry point for OS extension methods.
        /// </summary>
        /// <returns>an instance of type <see cref="IFluentOsExtensions"/>.</returns>
        public static IFluentOsExtensions Ex()
        {
            return new FluentOsExtensions();
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

        /// <summary>
        /// Retrieves the default <see cref="LogManager"/> instance.
        /// </summary>
        /// <returns>an instance of <see cref="LogManager"/>, or <c>null</c> if there was no registered default.</returns>
        public static LogManager Log()
        {
            return LogManager.GetManager();
        }
    }
}
