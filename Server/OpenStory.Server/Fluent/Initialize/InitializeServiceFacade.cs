using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeServiceFacade : NestedFacade<IInitializeFacade>, IInitializeServiceFacade
    {
        private readonly ServiceManager manager;

        public InitializeServiceFacade(IInitializeFacade parent)
            : base(parent)
        {
            manager = new ServiceManager();
        }

        #region Implementation of IInitializeServiceFacade

        public IInitializeServiceFacade WithLocal(IGameService local)
        {
            manager.RegisterComponent(ServiceManager.LocalServiceKey, local);
            return this;
        }

        public IInitializeServiceFacade WithNexus(INexusService nexus)
        {
            manager.RegisterComponent(ServiceManager.NexusServiceKey, nexus);
            return this;
        }

        #endregion

        public override IInitializeFacade Done()
        {
            this.manager.Initialize();

            ServiceManager.RegisterDefault(this.manager);

            return base.Done();
        }
    }
}