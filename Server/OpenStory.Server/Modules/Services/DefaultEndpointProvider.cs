using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace OpenStory.Server.Modules.Services
{
    internal sealed class DefaultEndpointProvider : IEndpointProvider
    {
        private static readonly NetTcpBinding DefaultBinding = new NetTcpBinding(SecurityMode.Transport);

        public static readonly IEndpointProvider Instance = new DefaultEndpointProvider();

        private DefaultEndpointProvider()
        {
        }

        #region Implementation of IEndpointProvider

        public ServiceEndpoint GetEndpoint<TServiceInterface>(Uri uri) where TServiceInterface : class
        {
            var contract = ContractDescription.GetContract(typeof(TServiceInterface));
            var binding = DefaultBinding;
            var address = new EndpointAddress(uri);

            var endpoint = new ServiceEndpoint(contract, binding, address);
            return endpoint;
        }

        #endregion
    }
}