using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Networking
{
    /// <summary>
    /// Represents a base class for all server clients.
    /// </summary>
    class ClientBase
    {
        /// <summary>
        /// The client session.
        /// </summary>
        public ISession Session { get; private set; }

        /// <summary>
        /// Initializes a new client with the given session object.
        /// </summary>
        /// <param name="session">The session object for this client.</param>
        /// <exception cref="ArgumentNullException">The exception is thrown when <paramref name="session"/> is null.</exception>
        protected ClientBase(ISession session)
        {
            if (session == null) throw new ArgumentNullException("session");
            this.Session = session;
        }
    }
}
