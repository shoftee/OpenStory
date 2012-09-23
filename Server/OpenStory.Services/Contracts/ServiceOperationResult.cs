using System;
using System.Runtime.Serialization;

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
        public Exception Error { get; private set; }

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
        public ServiceOperationResult(Exception error, ServiceState serviceState = ServiceState.Unknown)
            : this(true, error, serviceState)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceOperationResult"/>.
        /// </summary>
        /// <param name="failedLocally">Whether the operation failed locally.</param>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service.</param>
        public ServiceOperationResult(bool failedLocally, Exception error, ServiceState serviceState)
            : this()
        {
            this.FailedLocally = failedLocally;
            this.Error = error;
            this.ServiceState = serviceState;
        }

        /// <summary>
        /// Constructs a local result instance from a remote result.
        /// </summary>
        /// <param name="remoteResult">The result that was received from a remote service.</param>
        /// <returns>a transformed copy of the provided result.</returns>
        public static ServiceOperationResult FromRemoteResult(ServiceOperationResult remoteResult)
        {
            var result = new ServiceOperationResult(false, remoteResult.Error, remoteResult.ServiceState);
            return result;
        }
    }
}
