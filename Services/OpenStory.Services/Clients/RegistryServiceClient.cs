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
        public ServiceOperationResult TryRegisterService(ServiceConfiguration configuration, out Guid token)
        {
            var localToken = default(Guid);
            var result = ServiceOperationResult.Of(() => base.Channel.TryRegisterService(configuration, out localToken));
            token = localToken;
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult TryUnregisterService(Guid registrationToken)
        {
            var result = ServiceOperationResult.Of(() => base.Channel.TryUnregisterService(registrationToken));
            return result;
        }

        #endregion
    }
}
