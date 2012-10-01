using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides a fluent interface for managing an account service.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IAccountServiceFacade :
        IFluentInterface,
        IServiceGetterFacade<IAccountService>,
        INestedFacade<IServiceFacade>
    {
    }
}