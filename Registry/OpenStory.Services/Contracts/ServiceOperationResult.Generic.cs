using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Represents a generic result of a service call.
    /// </summary>
    /// <typeparam name="T">The type of the resulting value.</typeparam>
    [DataContract]
    public struct ServiceOperationResult<T> : IServiceOperationResult
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
        /// Gets the result of the operation.
        /// </summary>
        [DataMember]
        public T Result { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceOperationResult{T}"/>.
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
        /// Initializes a new instance of <see cref="ServiceOperationResult{T}"/>
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
        /// Initializes a new instance of <see cref="ServiceOperationResult{T}"/>.
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

            this.Result = result;
            this.OperationState = operationState;
            this.Error = error;
            this.ServiceState = serviceState;
        }

        /// <summary>
        /// Executes the provided call and catches possible communication exceptions.
        /// </summary>
        /// <param name="func">The service call to execute.</param>
        /// <returns>the possibly transformed operation result returned by the call.</returns>
        public static ServiceOperationResult<TResult> Of<TResult>(Func<ServiceOperationResult<TResult>> func)
        {
            try
            {
                var result = func();
                return FromRemoteResult(result);
            }
            catch (EndpointNotFoundException unreachable)
            {
                var result = LocalFailure<TResult>(unreachable);
                return result;
            }
            catch (TimeoutException timeout)
            {
                var result = LocalFailure<TResult>(timeout);
                return result;
            }
            catch (AddressAccessDeniedException accessDenied)
            {
                var result = new ServiceOperationResult<TResult>(default(TResult), OperationState.Refused, accessDenied, ServiceState.Unknown);
                return result;
            }
            catch (CommunicationException communicationException)
            {
                var result = RemoteFailure<TResult>(communicationException);
                return result;
            }
        }

        /// <summary>
        /// Constructs a local result instance from a remote result.
        /// </summary>
        /// <param name="remoteResult">The result that was received from a remote service.</param>
        /// <returns>a transformed copy of the provided result.</returns>
        private static ServiceOperationResult<TResult> FromRemoteResult<TResult>(ServiceOperationResult<TResult> remoteResult)
        {
            var actualOperationState = remoteResult.OperationState;
            if (actualOperationState == OperationState.FailedLocally)
            {
                actualOperationState = OperationState.FailedRemotely;
            }
            var result = new ServiceOperationResult<TResult>(remoteResult.Result, actualOperationState, remoteResult.Error, remoteResult.ServiceState);
            return result;
        }

        private static ServiceOperationResult<TResult> LocalFailure<TResult>(Exception error)
        {
            return new ServiceOperationResult<TResult>(default(TResult), OperationState.FailedLocally, error, ServiceState.Unknown);
        }

        private static ServiceOperationResult<TResult> RemoteFailure<TResult>(Exception error)
        {
            return new ServiceOperationResult<TResult>(default(TResult), OperationState.FailedRemotely, error, ServiceState.Unknown);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            string result;
            if (this.Error == null)
            {
                result = String.Format("Svc: {0}, Op: {1}, Result: {2}", this.ServiceState, this.OperationState, this.Result);
            }
            else
            {
                result = String.Format("Svc: {0}, Error: {1}", this.ServiceState, this.Error.Message);
            }
            return result;
        }
    }
}
