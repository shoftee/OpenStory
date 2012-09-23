using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Services.Contracts;

namespace OpenStory.Server.Management
{
    class NexusService : INexusService 
    {
        #region Implementation of INexusService

        public ServiceOperationResult TryGetAccountServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryGetAuthServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryGetChannelServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryGetWorldServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceOperationResult TryGetServiceConfiguration(Guid accessToken, out ServiceConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
