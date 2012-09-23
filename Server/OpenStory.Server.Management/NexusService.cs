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

        public ServiceOperationResult TryGetServiceConfiguration(Guid accessToken, out ServiceConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
