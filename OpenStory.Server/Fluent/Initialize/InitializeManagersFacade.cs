using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeManagersFacade<TManagerBase> : IInitializeManagersFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly IInitializeFacade parent;

        public InitializeManagersFacade(IInitializeFacade parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc />
        public IInitializeManagersFacade<TManagerBase> Manager<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            manager.Initialize();

            ManagerBase<TManagerBase>.RegisterManager(manager);
            return this;
        }

        /// <inheritdoc />
        public IInitializeManagerComponentsFacade<TManagerBase> WithComponents<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            manager.Initialize();

            return new InitializeManagerComponentsFacade<TManagerBase>(this, manager, false);
        }

        /// <inheritdoc />
        public IInitializeManagersFacade<TManagerBase> DefaultManager(TManagerBase manager)
        {
            manager.Initialize();

            ManagerBase<TManagerBase>.RegisterDefault(manager);
            return this;
        }

        /// <inheritdoc />
        public IInitializeManagerComponentsFacade<TManagerBase> DefaultWithComponents(TManagerBase manager)
        {
            manager.Initialize();

            return new InitializeManagerComponentsFacade<TManagerBase>(this, manager, true);
        }

        /// <inheritdoc />
        public IInitializeFacade Done()
        {
            return this.parent;
        }
    }
}
