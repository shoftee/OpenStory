using System;
using System.Runtime.Serialization;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Specifies the operational state of a game service.
    /// </summary>
    [Serializable]
    public enum ServiceState
    {
        /// <summary>
        /// The state of the service is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The service is not initialized.
        /// </summary>
        NotInitialized,

        /// <summary>
        /// The service is currently initializing.
        /// </summary>
        Initializing,

        /// <summary>
        /// The service is ready to start opertaion.
        /// </summary>
        Ready,

        /// <summary>
        /// The service is starting operation.
        /// </summary>
        Starting,

        /// <summary>
        /// The service is operating.
        /// </summary>
        Running,

        /// <summary>
        /// The service is shutting down.
        /// </summary>
        Stopping,
    }
}