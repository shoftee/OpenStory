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
        private NetworkSession NetworkSession { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress { get; private set; }

        public abstract IAccount AccountInfo { get; }

        /// <summary>
        /// Initializes a new client with the given network session object.
        /// </summary>
        /// <param name="session">The network session object for this client.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="session"/> is null.</exception>
        /// <exception cref="InvalidOperationException">The exception is thrown when <paramref name="session"/> is not open.</exception>
        protected AbstractClient(NetworkSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (session.SessionId == -1) throw new InvalidOperationException("This session is not open.");
            this.NetworkSession = session;
        }

        public void Disconnect()
        {
            this.NetworkSession.Close();
        }
    }
}
