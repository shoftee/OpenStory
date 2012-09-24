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
        /// Gets the state of the operation.
        /// </summary>
        [DataMember]
        public OperationState OperationState { get; private set; }

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
        public ServiceOperationResult(ServiceState serviceState)
            : this(OperationState.Success, null, serviceState)
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
            : this(OperationState.FailedLocally, error, serviceState)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceOperationResult"/>.
        /// </summary>
        /// <param name="operationState">The state of the operation.</param>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service.</param>
        public ServiceOperationResult(OperationState operationState, Exception error, ServiceState serviceState)
            : this()
        {
            this.OperationState = operationState;
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
            var actualOperationState = remoteResult.OperationState;
            if (actualOperationState == OperationState.FailedLocally)
            {
                actualOperationState = OperationState.FailedRemotely;
            }
            var result = new ServiceOperationResult(actualOperationState, remoteResult.Error, remoteResult.ServiceState);
            return result;
        }
    }
}
