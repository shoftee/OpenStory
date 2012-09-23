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
    /// <typeparam name="TServiceInterface">The type of the service reference.</typeparam>
    /// <typeparam name="TFluentFacade">The type of the implementing interface.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceGetterSetterFacade<TServiceInterface, TFluentFacade>
        where TFluentFacade : class
    {
        /// <summary>
        /// Gets the service reference.
        /// </summary>
        TServiceInterface Get();

        /// <summary>
        /// Sets the service reference.
        /// </summary>
        /// <param name="service">The service reference to set.</param>
        TFluentFacade Set(TServiceInterface service);
    }
}
