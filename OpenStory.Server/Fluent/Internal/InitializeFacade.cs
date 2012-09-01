using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Internal
{
    internal sealed class InitializeFacade : IInitializeFacade
    {
        /// <inheritdoc />
        public IInitializeManagersFacade<DataManager> DataManagers()
        {
            return new InitializeManagersFacade<DataManager>(this);
        }
    }
}
