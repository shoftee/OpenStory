using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to a game nexus service.
    /// </summary>
    public sealed class RegistryServiceClient : ClientBase<IRegistryService>, IRegistryService
    {
        /// <summary>
        /// Initialized a new instance of <see cref="RegistryServiceClient"/> with the specified endpoint address.
        /// </summary>
        /// <param name="uri">The URI of the service to connect to.</param>
        public RegistryServiceClient(Uri uri)
            : base(new NetTcpBinding(SecurityMode.Transport), new EndpointAddress(uri))
        {
        }

        #region IRegistryService Members

        /// <inheritdoc />
        public ServiceOperationResult TryRegisterAuthService(Uri uri, out Guid token)
        {
            var localToken = default(Guid);
            var result = HandleServiceCall(() => base.Channel.TryRegisterAuthService(uri, out localToken));
            token = localToken;
            return ServiceOperationResult.FromRemoteResult(result);
        }

        /// <inheritdoc />
        public ServiceOperationResult TryRegisterAccountService(Uri uri, out Guid token)
        {
            var localToken = default(Guid);
            var result = HandleServiceCall(() => base.Channel.TryRegisterAccountService(uri, out localToken));
            token = localToken;
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult TryRegisterWorldService(Uri uri, int worldId, out Guid token)
        {
            var localToken = default(Guid);
            var result = HandleServiceCall(() => base.Channel.TryRegisterWorldService(uri, worldId, out localToken));
            token = localToken;
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult TryRegisterChannelService(Uri uri, int worldId, int channelId, out Guid token)
        {
            var localToken = default(Guid);
            var result = HandleServiceCall(() => base.Channel.TryRegisterChannelService(uri, worldId, channelId, out localToken));
            token = localToken;
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult TryUnregisterService(Guid registrationToken)
        {
            var result = HandleServiceCall(() => base.Channel.TryUnregisterService(registrationToken));
            return result;
        }

        private static ServiceOperationResult HandleServiceCall(Func<ServiceOperationResult> func)
        {
            try
            {
                var result = func();
                return ServiceOperationResult.FromRemoteResult(result);
            }
            catch (EndpointNotFoundException unreachable)
            {
                var result = new ServiceOperationResult(unreachable);
                return result;
            }
            catch (TimeoutException timeout)
            {
                var result = new ServiceOperationResult(timeout);
                return result;
            }
            catch (AddressAccessDeniedException accessDenied)
            {
                var result = new ServiceOperationResult(OperationState.Refused, accessDenied, ServiceState.Unknown);
                return result;
            }
        }

        #endregion
    }
}
