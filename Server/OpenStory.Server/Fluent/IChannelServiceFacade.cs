using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing a channel service.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IChannelServiceFacade :
        IFluentInterface,
        IServiceGetterFacade<IChannelService>,
        INestedFacade<IServiceFacade>
    {
    }
}