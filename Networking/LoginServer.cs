using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using OpenMaple.Cryptography;

namespace OpenMaple.Networking
{
    class LoginServer
    {
        // TODO: Loading from property file?... or DB?
        public const int Port = 8484;

        /// <summary>
        /// Maximum number of characters on a single account.
        /// </summary>
        public const int MaxCharacters = 3;

        /// <summary>
        /// The name of the server.
        /// </summary>
        public const string ServerName = "OpenMS";

        public string EventMessage { get; set; }

        private static readonly LoginServer InternalInstance = new LoginServer();
        public static LoginServer Instance { get { return InternalInstance; } }

        private WorldManager worldManager;
        private LoginHandler loginHandler;
        private Acceptor acceptor;

        private LoginServer()
        {
            this.worldManager = new WorldManager();
            this.loginHandler = new LoginHandler();
            this.acceptor = new Acceptor(Port, AcceptHandler);
        }

        // TODO: FINISH THIS F5
        private static void AcceptHandler(Socket socket)
        {
            ISession session = SessionManager.GetNewSession(socket);
        }

        public void ShutDown(TimeSpan offset)
        {
            // TODO: Schedule shut down.
        }


    }

}
