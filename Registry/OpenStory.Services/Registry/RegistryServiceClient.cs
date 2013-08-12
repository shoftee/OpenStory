using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Registry
{
    /// <summary>
    /// Represents a client to a game nexus service.
    /// </summary>
    public sealed class RegistryServiceClient : ClientBase<IRegistryService>, IRegistryService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryServiceClient"/> class.
        /// </summary>
        public RegistryServiceClient()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryServiceClient"/> class with the specified endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint object for the service host.</param>
        public RegistryServiceClient(ServiceEndpoint serviceEndpoint)
            : base(serviceEndpoint)
        {
        }

        #region IRegistryService Members

        /// <inheritdoc />
        public ServiceOperationResult<Guid> RegisterService(ServiceConfiguration configuration)
        {
            var result = this.Call(() => this.Channel.RegisterService(configuration));
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult UnregisterService(Guid token)
        {
            var result = this.Call(() => this.Channel.UnregisterService(token));
            return result;
        }

        /// <inheritdoc />
        public ServiceOperationResult<Guid[]> GetRegistrations()
        {
            var result = this.Call(() => this.Channel.GetRegistrations());
            return result;
        }

        #endregion

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(Guid token)
        {
            var result = this.Call(() => this.Channel.GetServiceConfiguration(token));
            return result;
        }

        #endregion
    }
}
