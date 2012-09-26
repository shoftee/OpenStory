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
        /// <param name="operationState">The state of the operation.</param>
        /// <param name="error">The exception to assign to this result.</param>
        /// <param name="serviceState">The state of the service.</param>
        public ServiceOperationResult(OperationState operationState, Exception error, ServiceState serviceState)
            : this()
        {
            if (!Enum.IsDefined(typeof(OperationState), operationState))
            {
                throw new ArgumentOutOfRangeException("operationState", operationState, "'operationState' must have a valid OperationState enum value.");
            }
            if (!Enum.IsDefined(typeof(ServiceState), serviceState))
            {
                throw new ArgumentOutOfRangeException("serviceState", serviceState, "'serviceState' must have a valid ServiceState enum value.");
            }

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

        /// <summary>
        /// Executes the provided call and catches possible communication exceptions.
        /// </summary>
        /// <param name="func">The service call to execute.</param>
        /// <returns>the possibly transformed operation result returned by the call.</returns>
        public static ServiceOperationResult Of(Func<ServiceOperationResult> func)
        {
            try
            {
                var result = func();
                return FromRemoteResult(result);
            }
            catch (EndpointNotFoundException unreachable)
            {
                var result = LocalFailure(unreachable);
                return result;
            }
            catch (TimeoutException timeout)
            {
                var result = LocalFailure(timeout);
                return result;
            }
            catch (AddressAccessDeniedException accessDenied)
            {
                var result = new ServiceOperationResult(OperationState.Refused, accessDenied, ServiceState.Unknown);
                return result;
            }
            catch (CommunicationException communicationException)
            {
                var result = RemoteFailure(communicationException);
                return result;
            }
        }

        private static ServiceOperationResult LocalFailure(Exception error)
        {
            return new ServiceOperationResult(OperationState.FailedLocally, error, ServiceState.Unknown);
        }

        private static ServiceOperationResult RemoteFailure(Exception error)
        {
            return new ServiceOperationResult(OperationState.FailedRemotely, error, ServiceState.Unknown);
        }
    }
}
