using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Framework.Contracts;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Clients
{
    /// <summary>
    /// Represents a client to the nexus service.
    /// </summary>
    public sealed class NexusServiceClient : GameServiceClientBase<INexusService>, INexusService
    {
        private static readonly ContractDescription Contract = ContractDescription.GetContract(typeof(INexusService));
        private static readonly NetTcpBinding Binding = new NetTcpBinding(SecurityMode.Transport);

        /// <summary>
        /// Initializes a new instance of <see cref="NexusServiceClient"/>.
        /// </summary>
        public NexusServiceClient(Uri uri)
            : base(new ServiceEndpoint(Contract, Binding, new EndpointAddress(uri)))
        {
        }

        #region Implementation of INexusService

        /// <inheritdoc />
        public ServiceOperationResult<ServiceConfiguration> GetServiceConfiguration(Guid token)
        {
            var result = ServiceOperationResult<ServiceConfiguration>.Of(
                () => base.Channel.GetServiceConfiguration(token)
            );

            return result;
        }

        #endregion
    }
}