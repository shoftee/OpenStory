using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a generic result of a service call.
    /// </summary>
    /// <typeparam name="T">The type of the resulting value.</typeparam>
    [DataContract]
    public struct ServiceOperationResult<T> : IServiceOperationResult
    {
        [DataMember]
        private T result;

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
        /// Initializes a new instance of the <see cref="ServiceOperationResult{T}"/> struct.
        /// </summary>
        /// <remarks>
        /// You may use this constructor for successfully completed operations.
        /// </remarks>
        /// <param name="result">The result of the service operation.</param>
        /// <param name="serviceState">The state of the service, if known.</param>
        public ServiceOperationResult(T result, ServiceState serviceState)
            : this(result, OperationState.Success, null, serviceState)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceOperationResult{T}"/> struct
        /// </summary>
        /// <remarks>
        /// You may use this constructor for operations that completed without a valid result.
        /// </remarks>
        /// <param name="operationState">The state of the operation.</param>
        /// <param name="serviceState">The state of the service, if known.</param>
        public ServiceOperationResult(OperationState operationState, ServiceState serviceState)
            : this(default(T), operationState, null, serviceState)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceOperationResult{T}"/> struct.
        /// </summary>
        /// <param name="result">The result of the service operation.</param>
        /// <param name="operationState">The state of the operation.</param>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service.</param>
        public ServiceOperationResult(T result, OperationState operationState, Exception error, ServiceState serviceState)
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

            this.result = result;
            this.OperationState = operationState;
            this.Error = error;
            this.ServiceState = serviceState;
        }

        /// <summary>
        /// Gets the result of this service operation.
        /// </summary>
        /// <param name="throwOnError">Denotes whether to throw an exception if there was an error.</param>
        /// <exception>Thrown if the operation failed and <paramref name="throwOnError"/> is set to <see langword="false"/>.</exception>
        /// <returns> the result of the wrapped service operation, or the default value for <typeparamref name="T"/> if the operation failed.</returns>
        public T GetResult(bool throwOnError = true)
        {
            if (this.Error != null && throwOnError)
            {
                throw Error;
            }

            return this.result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if (this.Error == null)
            {
                return string.Format("Svc: {0}, Op: {1}, Result: {2}", this.ServiceState, this.OperationState, this.result);
            }
            else
            {
                return string.Format("Svc: {0}, Error: {1}", this.ServiceState, this.Error.Message);
            }
        }
    }
}
