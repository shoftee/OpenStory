using System;
using OpenMaple.Data;
using OpenMaple.Networking;

namespace OpenMaple.Server
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// This class is abstract.
    /// </summary>
    abstract class AbstractClient
    {
        /// <summary>
        /// The client's network session.
        /// </summary>
        public INetworkSession NetworkSession { get; private set; }

        public abstract IAccount AccountInfo { get; }

        /// <summary>
        /// Initializes a new client with the given network session object.
        /// </summary>
        /// <param name="session">The network session object for this client.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="session"/> is null.</exception>
        protected AbstractClient(INetworkSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.NetworkSession = session;
        }

        public void Disconnect()
        {
            this.NetworkSession.Close();
        }
    }
}
