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

        public ServiceState TryGetAccountServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryGetAuthServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryGetChannelServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryGetWorldServiceUri(Guid accessToken, out Uri uri)
        {
            throw new NotImplementedException();
        }

        public ServiceState TryGetServiceConfiguration(Guid accessToken, out ServiceConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
