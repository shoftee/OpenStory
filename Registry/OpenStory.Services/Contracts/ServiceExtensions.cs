using System;
using System.ServiceModel;

namespace OpenStory.Services.Contracts
{
    /// <summary>
    /// Contains extensions methods for WCF functionality.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Calls a remote method and handles operation errors.
        /// </summary>
        /// <remarks>
        /// If the remote call fails for whatever reason, this method will call <see cref="ClientBase{TChannel}.Abort()"/> on the provided client.
        /// </remarks>
        /// <typeparam name="TResult">The type of the result from the remote operation.</typeparam>
        /// <typeparam name="TChannel">The type of the remote channel.</typeparam>
        /// <param name="client">The client that is calling the remote method.</param>
        /// <param name="func">The call to execute.</param>
        /// <returns>a <see cref="ServiceOperationResult{TResult}"/> describing the result of the operation.</returns>
        public static ServiceOperationResult<TResult> Call<TChannel, TResult>(
            this ClientBase<TChannel> client, Func<ServiceOperationResult<TResult>> func)
            where TChannel : class
        {
            ServiceOperationResult<TResult> result;
            try
            {
                result = func().ToLocal();
            }
            catch (EndpointNotFoundException unreachable)
            {
                client.Abort();
                result = LocalFailure<TResult>(unreachable);
            }
            catch (TimeoutException timeout)
            {
                client.Abort();
                result = LocalFailure<TResult>(timeout);
            }
            catch (AddressAccessDeniedException accessDenied)
            {
                client.Abort();
                result = new ServiceOperationResult<TResult>(default(TResult), OperationState.Refused, accessDenied, ServiceState.Unknown);
            }
            catch (CommunicationException communicationException)
            {
                client.Abort();
                result = RemoteFailure<TResult>(communicationException);
            }

            return result;
        }

        /// <summary>
        /// Constructs a local result instance from a remote result.
        /// </summary>
        /// <param name="remoteResult">The result that was received from a remote service.</param>
        /// <returns>a transformed copy of the provided result.</returns>
        private static ServiceOperationResult<TResult> ToLocal<TResult>(this ServiceOperationResult<TResult> remoteResult)
        {
            var actualOperationState = remoteResult.OperationState;
            if (actualOperationState == OperationState.FailedLocally)
            {
                actualOperationState = OperationState.FailedRemotely;
            }

            var result = new ServiceOperationResult<TResult>(remoteResult.GetResult(false), actualOperationState, remoteResult.Error, remoteResult.ServiceState);
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

        /// <summary>
        /// Calls a remote method and handles operation errors.
        /// </summary>
        /// <remarks>
        /// If the remote call fails for whatever reason, this method will call <see cref="ClientBase{TChannel}.Abort()"/> on the provided client.
        /// </remarks>
        /// <typeparam name="TChannel">The type of the remote channel.</typeparam>
        /// <param name="client">The client that is calling the remote method.</param>
        /// <param name="func">The call to execute.</param>
        /// <returns>a <see cref="ServiceOperationResult"/> describing the result of the operation.</returns>
        public static ServiceOperationResult Call<TChannel>(
            this ClientBase<TChannel> client, Func<ServiceOperationResult> func)
            where TChannel : class
        {
            ServiceOperationResult result;
            try
            {
                result = func().ToLocal();
            }
            catch (EndpointNotFoundException unreachable)
            {
                client.Abort();
                result = LocalFailure(unreachable);
            }
            catch (TimeoutException timeout)
            {
                client.Abort();
                result = LocalFailure(timeout);
            }
            catch (AddressAccessDeniedException accessDenied)
            {
                client.Abort();
                result = new ServiceOperationResult(OperationState.Refused, accessDenied, ServiceState.Unknown);
            }
            catch (CommunicationException communicationException)
            {
                client.Abort();
                result = RemoteFailure(communicationException);
            }

            return result;
        }

        /// <summary>
        /// Constructs a local result instance from a remote result.
        /// </summary>
        /// <param name="remoteResult">The result that was received from a remote service.</param>
        /// <returns>a transformed copy of the provided result.</returns>
        private static ServiceOperationResult ToLocal(this ServiceOperationResult remoteResult)
        {
            var actualOperationState = remoteResult.OperationState;
            if (actualOperationState == OperationState.FailedLocally)
            {
                actualOperationState = OperationState.FailedRemotely;
            }

            var result = new ServiceOperationResult(actualOperationState, remoteResult.Error, remoteResult.ServiceState);
            return result;
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