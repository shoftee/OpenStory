using System.ComponentModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Fluent
{
    /// <summary>
    /// Provides methods for initializing 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IInitializeServiceFacade : INestedFacade<IInitializeFacade>
    {
        /// <summary>
        /// Registers the "LocalService" component of the ServiceManager.
        /// </summary>
        /// <param name="local">The service reference to register.</param>
        IInitializeServiceFacade WithLocal(IManagedService local);

        /// <summary>
        /// Registers the "NexusService" component of the ServiceManager.
        /// </summary>
        /// <param name="nexus">The service reference to register.</param>
        IInitializeServiceFacade WithNexus(INexusService nexus);
    }
}