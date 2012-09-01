using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Internal
{
    internal sealed class InitializeFacade : IInitializeFacade
    {
        /// <inheritdoc />
        public IInitializeManagerFacade<DataManager> DataManager()
        {
            return new InitializeManagerFacade<DataManager>(this);
        }
    }
}
