using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for initializing OpenStory.
    /// </summary>
    public interface IInitializeFacade
    {
        /// <summary>
        /// The entry point to the <see cref="DataManager"/> initialization facade.
        /// </summary>
        /// <returns>an instance of type <see cref="IInitializeManagerFacade{DataManager}"/>.</returns>
        IInitializeManagerFacade<DataManager> DataManager();
    }
}