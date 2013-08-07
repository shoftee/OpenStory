using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Contains information about a ServiceStateChanged event.
    /// </summary>
    public sealed class ServiceStateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new state of the service.
        /// </summary>
        public ServiceState NewState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStateEventArgs"/> class.
        /// </summary>
        /// <param name="newState">The new state of the service.</param>
        internal ServiceStateEventArgs(ServiceState newState)
        {
            this.NewState = newState;
        }
    }
}