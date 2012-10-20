using System.Net;
using System.Runtime.Serialization;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents a set of server configuration settings.
    /// </summary>
    [DataContract]
    public abstract class ServerConfiguration
    {
        /// <summary>
        /// Gets the entry point definition for the server.
        /// </summary>
        [DataMember]
        public IPEndPoint Endpoint { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerConfiguration"/>.
        /// </summary>
        /// <param name="endpoint">The entry point definition for the server.</param>
        protected ServerConfiguration(IPEndPoint endpoint)
        {
            this.Endpoint = endpoint;
        }
    }
}