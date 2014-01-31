using System;
using OpenStory.Framework.Contracts;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Provides methods for starting and stopping a server.
    /// </summary>
    public interface IServerProcess : IRegisteredProcess, IConfigurableService
    {
        /// <summary>
        /// Occurs when a new server session has started.
        /// </summary>
        event EventHandler<ServerSessionEventArgs> ConnectionOpened;
    }
}
