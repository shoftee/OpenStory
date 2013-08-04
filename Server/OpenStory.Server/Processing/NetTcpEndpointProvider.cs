using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using OpenStory.Framework.Contracts;

namespace OpenStory.Server.Processing
{
    internal sealed class NetTcpEndpointProvider : IEndpointProvider
    {
        #region Implementation of IEndpointProvider

        public ServiceEndpoint GetEndpoint<TServiceInterface>(Uri uri) 
            where TServiceInterface : class
        {
            var contract = ContractDescription.GetContract(typeof(TServiceInterface));
            var binding = new NetTcpBinding(SecurityMode.Transport);
            var address = new EndpointAddress(uri);

            var endpoint = new ServiceEndpoint(contract, binding, address);
            return endpoint;
        }

        #endregion
    }
}