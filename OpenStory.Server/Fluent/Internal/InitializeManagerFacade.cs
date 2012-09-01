using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Internal
{
    internal sealed class InitializeManagerFacade<TManagerBase> : IInitializeManagerFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly IInitializeFacade parent;

        public InitializeManagerFacade(IInitializeFacade parent)
        {
            this.parent = parent;
        }

        /// <inheritdoc />
        public IInitializeManagerFacade<TManagerBase> Manager<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            manager.Initialize();

            ManagerBase<TManagerBase>.RegisterManager(manager);
            return this;
        }

        public IInitializeManagerComponentsFacade<TManagerBase> WithComponents<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            manager.Initialize();

            return new InitializeManagerComponentsFacade<TManagerBase>(this, manager, false);
        }

        /// <inheritdoc />
        public IInitializeFacade DefaultManager(TManagerBase manager)
        {
            manager.Initialize();

            ManagerBase<TManagerBase>.RegisterDefault(manager);
            return this.parent;
        }

        public IInitializeManagerComponentsFacade<TManagerBase> DefaultWithComponents(TManagerBase manager)
        {
            manager.Initialize();

            return new InitializeManagerComponentsFacade<TManagerBase>(this, manager, true);
        }
    }
}