using System;
using OpenStory.Services.Contracts;

namespace OpenStory.Services.Management
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
