using System;
using System.ComponentModel;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Represents connection information for a nexus service.
    /// </summary>
    [Localizable(true)]
    public sealed class NexusConnection
    {
        /// <summary>
        /// Gets the access token required to communicate with the Nexus service.
        /// </summary>
        public Guid AccessToken { get; private set; }

        /// <summary>
        /// Gets the URI of the Nexus service.
        /// </summary>
        public Uri NexusUri { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NexusConnection"/> class.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="nexusUri">The URI of the nexus service.</param>
        public NexusConnection(Guid accessToken, Uri nexusUri)
        {
            this.AccessToken = accessToken;
            this.NexusUri = nexusUri;
        }
    }
}