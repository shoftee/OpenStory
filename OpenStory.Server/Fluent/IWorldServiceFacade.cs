using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing a world service.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IWorldServiceFacade :
        IFluentInterface,
        IServiceGetterSetterFacade<IWorldService, IWorldServiceFacade>,
        INestedFacade<IServiceFacade>
    {
    }
}