using OpenStory.Server.Modules;
using OpenStory.Server.Modules.Logging;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeFacade : IInitializeFacade
    {
        /// <inheritdoc />
        public IInitializeManagersFacade<TManager> Managers<TManager>()
            where TManager : ManagerBase
        {
            return new InitializeManagersFacade<TManager>(this);
        }

        /// <inheritdoc />
        public IInitializeFacade Manager<TManager>(TManager instance)
            where TManager : ManagerBase
        {
            ManagerBase<TManager>.RegisterDefault(instance);
            return this;
        }

        /// <inheritdoc />
        public IInitializeFacade Logger(ILogger logger)
        {
            var instance = new LogManager();
            LogManager.RegisterDefault(instance);
            instance.RegisterComponent(LogManager.LoggerKey, logger);
            instance.Initialize();
            return this;
        }

        public IInitializeServiceFacade Services()
        {
            return new InitializeServiceFacade(this);
        }
    }
}
