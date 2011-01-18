using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using OpenStory.Server.Networking;

namespace OpenStory.Server
{
    /// <summary>
    /// A base class for Server instances.
    /// </summary>
    public abstract class AbstractServer
    {
        private Acceptor connectionAcceptor;

        /// <summary>
        /// Initializes a new instance of AbstractServer and binds the internal acceptor to the given port.
        /// </summary>
        /// <param name="port">The port to listen on.</param>
        protected AbstractServer(int port)
        {
            connectionAcceptor = new Acceptor(port, HandleAccept);
        }

        /// <summary>
        /// Initiates the connection accept routines.
        /// </summary>
        public void StartAccepting()
        {
            connectionAcceptor.Start();
        }

        /// <summary>
        /// This method is a callback for the internal socket acceptor.
        /// </summary>
        /// <param name="socket">The socket to handle the connection of.</param>
        protected abstract void HandleAccept(Socket socket);
    }
}
