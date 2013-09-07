using System;
using OpenStory.Services;
using OpenStory.Services.Contracts;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for starting and stopping a server.
    /// </summary>
    public interface IServerProcess : IConfigurableService
    {
        /// <summary>
        /// Occurs when a new server session has started.
        /// </summary>
        event EventHandler<ServerSessionEventArgs> ConnectionOpened;

        /// <summary>
        /// Gets whether the server is running or not.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Starts the server.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();
    }
}
