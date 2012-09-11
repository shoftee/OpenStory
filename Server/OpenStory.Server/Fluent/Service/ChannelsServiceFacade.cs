using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent.Service
{
    internal sealed class ChannelsServiceFacade : NestedFacade<IServiceFacade>, IChannelsServiceFacade
    {
        public ChannelsServiceFacade(IServiceFacade parent)
            : base(parent)
        {
        }

        #region Implementation of IChannelsServiceFacade

        public IChannelService Get(int id)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}