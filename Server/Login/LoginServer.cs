using System.Collections.Generic;
using System.Net.Sockets;
using OpenMaple.Networking;

namespace OpenMaple.Server.Login
{
    public class LoginServer : ILoginServer
    {
        // TODO: Loading from property file?... or DB?
        public const int Port = 8484;

        /// <summary>
        /// Initial maximum number of characters on a single account.
        /// </summary>
        public const int MaxCharacters = 3;

        /// <summary>
        /// The name of the server.
        /// </summary>
        public const string ServerName = "OpenMS";

        public string EventMessage { get; set; }

        private static readonly LoginServer InternalInstance = new LoginServer();
        public static ILoginServer Instance { get { return InternalInstance; } }
        private LoginServer()
        {
            this.worldManager = new WorldManager();
            this.acceptor = new Acceptor(Port, this.HandleAccept);
            this.clients = new List<LoginClient>();

            this.acceptor.Start();
        }

        private readonly WorldManager worldManager;
        private readonly Acceptor acceptor;

        private readonly List<LoginClient> clients;

        // TODO: FINISH THIS F5
        private void HandleAccept(Socket socket)
        {
            NetworkSession networkSession = NetworkSession.New(socket);
            LoginClient newClient = new LoginClient(networkSession, this);
            this.clients.Add(newClient);
        }

        public IWorld GetWorldById(int worldId)
        {
            return this.worldManager.GetWorldById(worldId);
        }
    }
}
