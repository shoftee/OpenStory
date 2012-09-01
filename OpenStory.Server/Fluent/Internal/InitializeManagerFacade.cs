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
        public IInitializeManagerFacade<TManagerBase> With<TManager>(TManager manager)
            where TManager : TManagerBase
        {
            manager.Initialize();

            ManagerBase<TManagerBase>.RegisterManager(manager);
            return this;
        }

        /// <inheritdoc />
        public IInitializeFacade WithDefault(TManagerBase manager)
        {
            manager.Initialize();

            ManagerBase<TManagerBase>.RegisterDefault(manager);
            return this.parent;
        }
    }
}