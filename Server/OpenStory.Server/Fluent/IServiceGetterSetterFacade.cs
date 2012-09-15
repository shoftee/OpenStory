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
    /// <typeparam name="TManagedService">The type of the service reference.</typeparam>
    /// <typeparam name="TFluentFacade">The type of the implementing interface.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceGetterSetterFacade<TManagedService, TFluentFacade>
        where TManagedService : class, IManagedService
        where TFluentFacade : class
    {
        /// <summary>
        /// Gets the service reference.
        /// </summary>
        TManagedService Get();

        /// <summary>
        /// Sets the service reference.
        /// </summary>
        /// <param name="service">The service reference to set.</param>
        TFluentFacade Set(TManagedService service);
    }
}
