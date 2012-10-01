using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing an authentication service.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IAuthServiceFacade :
        IFluentInterface,
        IServiceGetterFacade<IAuthService>,
        INestedFacade<IServiceFacade>
    {
    }
}