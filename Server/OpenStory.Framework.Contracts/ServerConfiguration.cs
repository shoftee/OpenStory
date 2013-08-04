using System.Net;
using System.Runtime.Serialization;

namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Represents a set of server configuration settings.
    /// </summary>
    [DataContract]
    public class ServerConfiguration
    {
        /// <summary>
        /// Gets the entry point definition for the server.
        /// </summary>
        [DataMember]
        public IPEndPoint Endpoint { get; private set; }

        /// <summary>
        /// Get the game version for the server.
        /// </summary>
        [DataMember]
        public ushort Version { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">The object containing the configuration values.</param>
        public ServerConfiguration(ServiceConfiguration configuration)
        {
            this.Endpoint = configuration.Get<IPEndPoint>("Endpoint", true);
            this.Version = configuration.Get<ushort>("Version", true);
        }
    }
}