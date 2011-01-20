using System;
using OpenStory.Networking;

namespace OpenStory.Emulation
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// This class is abstract.
    /// </summary>
    internal abstract class AbstractClient
    {
        /// <summary>
        /// Initializes a new client with the given network session object.
        /// </summary>
        /// <param name="session">The network session object for this client.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="session"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="session"/> is not open.</exception>
        protected AbstractClient(NetworkSession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (session.SessionId == -1) throw new InvalidOperationException("This session is not open.");
            this.NetworkSession = session;
        }

        /// <summary>
        /// The client's network session.
        /// </summary>
        private NetworkSession NetworkSession { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RemoteAddress { get; private set; }

        public abstract IAccount AccountInfo { get; }

        public void Disconnect()
        {
            this.NetworkSession.Close();
        }
    }
}