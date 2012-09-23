using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents the result of a service call.
    /// </summary>
    [DataContract]
    public struct ServiceOperationResult
    {
        /// <summary>
        /// Gets whether the operation failed locally.
        /// </summary>
        [DataMember]
        public bool FailedLocally { get; private set; }

        /// <summary>
        /// Gets the exception that was thrown, if any.
        /// </summary>
        [DataMember]
        public CommunicationException Error { get; private set; }

        /// <summary>
        /// Gets the state of the called game service.
        /// </summary>
        [DataMember]
        public ServiceState ServiceState { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceOperationResult"/>.
        /// </summary>
        /// <remarks>
        /// You may use this constructor for successfully completed operations.
        /// </remarks>
        /// <param name="serviceState">The state of the service, if known.</param>
        public ServiceOperationResult(ServiceState serviceState = ServiceState.Unknown)
            : this(false, null, serviceState)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceOperationResult"/>.
        /// </summary>
        /// <remarks>
        /// You may use this constructor for local failures.
        /// </remarks>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service, if known.</param>
        public ServiceOperationResult(CommunicationException error, ServiceState serviceState = ServiceState.Unknown)
            : this(true, error, serviceState)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceOperationResult"/>.
        /// </summary>
        /// <param name="failedLocally">Whether the operation failed locally.</param>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service.</param>
        public ServiceOperationResult(bool failedLocally, CommunicationException error, ServiceState serviceState)
            : this()
        {
            this.FailedLocally = failedLocally;
            this.Error = error;
            this.ServiceState = serviceState;
        }
    }
}
