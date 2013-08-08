using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    internal sealed class NetTcpEndpointProvider : IEndpointProvider
    {
        #region Implementation of IEndpointProvider

        public ServiceEndpoint GetEndpoint(Type serviceType, Uri uri)
        {
            var contract = ContractDescription.GetContract(serviceType);
            var binding = new NetTcpBinding(SecurityMode.Transport);
            var address = new EndpointAddress(uri);

            var endpoint = new ServiceEndpoint(contract, binding, address);
            return endpoint;
        }

        #endregion
    }
}