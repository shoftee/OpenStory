using System;
using OpenStory.Services;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for retrieving <see cref="ServiceConfiguration"/>s.
    /// </summary>
    public interface IServiceConfigurationProvider
    {
        /// <summary>
        /// Gets a <see cref="ServiceConfiguration"/>.
        /// </summary>
        /// <param name="nexusConnectionInfo">The connection information to use when retrieving the configuration.</param>
        /// <returns>a <see cref="ServiceConfiguration"/> object.</returns>
        ServiceConfiguration GetConfiguration(NexusConnectionInfo nexusConnectionInfo);
    }
}
