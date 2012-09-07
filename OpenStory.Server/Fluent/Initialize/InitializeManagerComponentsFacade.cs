using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeManagerComponentsFacade<TManagerBase> : IInitializeManagerComponentsFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly InitializeManagersFacade<TManagerBase> parent;
        private readonly TManagerBase manager;
        private readonly bool registerDefault;

        public InitializeManagerComponentsFacade(InitializeManagersFacade<TManagerBase> parent, TManagerBase manager, bool registerDefault)
        {
            this.parent = parent;
            this.manager = manager;
            this.registerDefault = registerDefault;
        }

        /// <inheritdoc />
        public IInitializeManagerComponentsFacade<TManagerBase> Component(string name, object instance)
        {
            this.manager.RegisterComponent(name, instance);
            return this;
        }

        /// <inheritdoc />
        public IInitializeManagersFacade<TManagerBase> Done()
        {
            if (this.registerDefault)
            {
                ManagerBase<TManagerBase>.RegisterDefault(this.manager);
            }
            else
            {
                ManagerBase<TManagerBase>.RegisterManager(this.manager);
            }

            return this.parent;
        }
    }
}