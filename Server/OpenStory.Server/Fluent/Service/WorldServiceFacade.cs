using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class WorldServiceFacade : NestedFacade<IServiceFacade>, IWorldServiceFacade
    {
        public WorldServiceFacade(IServiceFacade parent)
            : base(parent)
        {
        }

        #region Implementation of IServiceGetterFacade<IWorldService,IWorldServiceFacade>

        public IWorldService Get()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}