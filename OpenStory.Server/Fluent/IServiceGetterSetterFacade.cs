using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for getting and setting a service reference.
    /// </summary>
    /// <typeparam name="TGameService">The type of the service reference.</typeparam>
    /// <typeparam name="TFluentFacade">The type of the implementing interface.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceGetterSetterFacade<TGameService, TFluentFacade>
        where TGameService : class, IGameService
        where TFluentFacade : class
    {
        /// <summary>
        /// Gets the service reference.
        /// </summary>
        TGameService Get();

        /// <summary>
        /// Sets the service reference.
        /// </summary>
        /// <param name="service">The service reference to set.</param>
        TFluentFacade Set(TGameService service);
    }
}
