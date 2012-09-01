using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Internal
{
    internal sealed class InitializeManagerComponentsFacade<TManagerBase> : IInitializeManagerComponentsFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly InitializeManagerFacade<TManagerBase> parent;
        private readonly TManagerBase manager;
        private readonly bool registerDefault;

        public InitializeManagerComponentsFacade(InitializeManagerFacade<TManagerBase> parent, TManagerBase manager, bool registerDefault)
        {
            this.parent = parent;
            this.manager = manager;
            this.registerDefault = registerDefault;
        }

        public IInitializeManagerComponentsFacade<TManagerBase> Component(string name, object instance)
        {
            this.manager.RegisterComponent(name, instance);
            return this;
        }

        public IInitializeManagerFacade<TManagerBase> Done()
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