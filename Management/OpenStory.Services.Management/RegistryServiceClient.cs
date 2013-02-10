using System;
using System.ServiceModel;
using OpenStory.Services.Contracts;
using OpenStory.Services.Registry;

namespace OpenStory.Services.Management
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
        public ServiceOperationResult<Guid> RegisterService(ServiceConfiguration configuration)
        {
            var result = ServiceOperationResult<Guid>.Of(() => base.Channel.RegisterService(configuration));
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult UnregisterService(Guid token)
        {
            var result = ServiceOperationResult.Of(() => base.Channel.UnregisterService(token));
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult<Guid[]> GetRegistrations()
        {
            var result = ServiceOperationResult<Guid[]>.Of(() => base.Channel.GetRegistrations());
            return result;
        }

        #endregion

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(Guid token)
        {
            var result = ServiceOperationResult<ServiceConfiguration>.Of(() => base.Channel.GetServiceConfiguration(token));
            return result;
        }

        #endregion
    }
}
