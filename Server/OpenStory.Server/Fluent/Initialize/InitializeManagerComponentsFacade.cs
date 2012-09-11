using OpenStory.Server.Modules;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeManagerComponentsFacade<TManagerBase> :
        NestedFacade<IInitializeManagersFacade<TManagerBase>>,
        IInitializeManagerComponentsFacade<TManagerBase>
        where TManagerBase : ManagerBase
    {
        private readonly TManagerBase manager;
        private readonly bool registerDefault;

        public InitializeManagerComponentsFacade(IInitializeManagersFacade<TManagerBase> parent, TManagerBase manager, bool registerDefault)
            : base(parent)
        {
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
        public override IInitializeManagersFacade<TManagerBase> Done()
        {
            if (this.registerDefault)
            {
                ManagerBase<TManagerBase>.RegisterDefault(this.manager);
            }
            else
            {
                ManagerBase<TManagerBase>.RegisterManager(this.manager);
            }

            this.manager.Initialize();

            return base.Done();
        }
    }
}