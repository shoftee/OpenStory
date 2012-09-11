using OpenStory.Server.Data;
using OpenStory.Server.Diagnostics;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeFacade : IInitializeFacade
    {
        /// <inheritdoc />
        public IInitializeManagersFacade<DataManager> DataManagers()
        {
            return new InitializeManagersFacade<DataManager>(this);
        }

        /// <inheritdoc />
        public IInitializeFacade Logger(ILogger logger)
        {
            var logManager = LogManager.GetManager();
            logManager.RegisterComponent(LogManager.LoggerKey, logger);
            logManager.Initialize();
            return this;
        }

        public IInitializeServiceFacade Services()
        {
            return new InitializeServiceFacade(this);
        }
    }
}
