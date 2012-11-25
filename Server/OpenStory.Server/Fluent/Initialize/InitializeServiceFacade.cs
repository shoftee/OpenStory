using OpenStory.Server.Modules.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Initialize
{
    internal sealed class InitializeServiceFacade : NestedFacade<IInitializeFacade>, IInitializeServiceFacade
    {
        private readonly ServiceManager manager;

        public InitializeServiceFacade(IInitializeFacade parent)
            : base(parent)
        {
            this.manager = new ServiceManager();
        }

        #region Implementation of IInitializeServiceFacade

        public IInitializeServiceFacade Host<TGameService>(TGameService local)
            where TGameService : class, IGameService
        {
            this.manager.RegisterComponent(ServiceManager.LocalServiceKey, local);
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