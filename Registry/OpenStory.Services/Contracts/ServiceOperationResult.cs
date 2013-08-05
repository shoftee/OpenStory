using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents the result of a service call.
    /// </summary>
    [DataContract]
    public struct ServiceOperationResult : IServiceOperationResult
    {
        /// <inheritdoc />
        [DataMember]
        public OperationState OperationState { get; private set; }

        /// <inheritdoc />
        [DataMember]
        public Exception Error { get; private set; }

        /// <inheritdoc />
        [DataMember]
        public ServiceState ServiceState { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceOperationResult"/> struct.
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
        /// Initializes a new instance of the <see cref="ServiceOperationResult"/> struct.
        /// </summary>
        /// <remarks>
        /// You may use this constructor for operations that completed without a valid result.
        /// </remarks>
        /// <param name="operationState">The state of the operation.</param>
        /// <param name="serviceState">The state of the service, if known.</param>
        public ServiceOperationResult(OperationState operationState, ServiceState serviceState)
            : this(operationState, null, serviceState)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceOperationResult"/> struct.
        /// </summary>
        /// <param name="operationState">The state of the operation.</param>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service.</param>
        public ServiceOperationResult(OperationState operationState, Exception error, ServiceState serviceState)
            : this()
        {
            if (!Enum.IsDefined(typeof(OperationState), operationState))
            {
                throw new InvalidEnumArgumentException("operationState", (int)operationState, typeof(OperationState));
            }

            if (!Enum.IsDefined(typeof(ServiceState), serviceState))
            {
                throw new InvalidEnumArgumentException("serviceState", (int)serviceState, typeof(ServiceState));
            }

            this.OperationState = operationState;
            this.Error = error;
            this.ServiceState = serviceState;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.Error == null)
            {
                return string.Format("Svc: {0}, Op: {1}", this.ServiceState, this.OperationState);
            }
            else
            {
                return string.Format("Svc: {0}, Error: {1}", this.ServiceState, this.Error.Message);
            }
        }
    }
}
