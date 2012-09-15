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
        /// Gets the entry point address that the server should listen on.
        /// </summary>
        [DataMember]
        public IPAddress Address { get; private set; }

        /// <summary>
        /// Gets the entry point port that the server should listen on.
        /// </summary>
        [DataMember]
        public int Port { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ServerConfiguration"/>.
        /// </summary>
        /// <param name="address">The entry point's IP address.</param>
        /// <param name="port">The entry point's port.</param>
        protected ServerConfiguration(IPAddress address, int port)
        {
            this.Address = address;
            this.Port = port;
        }
    }
}