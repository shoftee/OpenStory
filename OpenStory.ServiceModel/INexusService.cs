using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace OpenStory.ServiceModel
{
    /// <summary>
    /// Provides methods to register and list game services.
    /// </summary>
    /// <typeparam name="TGameService">The concrete implementation of <see cref="IGameService"/>.</typeparam>
    [ServiceContract(Namespace = null)]
    public interface INexusService<out TGameService> : IGameService
        where TGameService : class, IGameService
    {
        /// <summary>
        /// Gets the list of <see cref="TGameService"/> instances.
        /// </summary>
        IEnumerable<TGameService> Services
        {
            [OperationContract]
            get;
        }

        /// <summary>
        /// Registers the <typeparam name="TGameService"/> at the specified URI into the Nexus.
        /// </summary>
        /// <param name="serviceUri">The URI of the service to register.</param>
        /// <returns>a <see cref="string"/> token for the registered service.</returns>
        [OperationContract]
        string RegisterService(Uri serviceUri);
    }
}
