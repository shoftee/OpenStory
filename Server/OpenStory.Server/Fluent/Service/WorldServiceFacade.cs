using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class WorldServiceFacade : NestedFacade<IServiceFacade>, IWorldServiceFacade
    {
        public WorldServiceFacade(IServiceFacade parent)
            : base(parent)
        {
        }

        #region Implementation of IServiceGetterSetterFacade<IWorldService,IWorldServiceFacade>

        public IWorldService Get()
        {
            throw new System.NotImplementedException();
        }

        public IWorldServiceFacade Set(IWorldService service)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}