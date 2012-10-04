using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class ChannelServiceFacade : NestedFacade<IServiceFacade>, IChannelServiceFacade
    {
        public ChannelServiceFacade(IServiceFacade parent)
            : base(parent)
        {
            throw new System.NotImplementedException();
        }

        #region Implementation of IServiceGetterFacade<IChannelService,IChannelServiceFacade>

        public IChannelService Get()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}