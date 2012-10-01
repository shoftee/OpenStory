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
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IServiceGetterFacade<TServiceInterface>
        where TServiceInterface : IGameService
    {
        /// <summary>
        /// Gets the service reference.
        /// </summary>
        TServiceInterface Get();
    }
}
