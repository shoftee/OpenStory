using System.Collections.Generic;
using System.Net.Sockets;
using OpenStory.Server.Networking;

namespace OpenStory.Server.Login
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

        private static readonly LoginServer InternalInstance = new LoginServer();

        private readonly Acceptor acceptor;

        private readonly List<LoginClient> clients;
        private readonly WorldManager worldManager;

        private LoginServer()
        {
            this.worldManager = new WorldManager();
            this.acceptor = new Acceptor(Port, this.HandleAccept);
            this.clients = new List<LoginClient>();

            this.acceptor.Start();
        }

        public string EventMessage { get; set; }

        public static ILoginServer Instance
        {
            get { return InternalInstance; }
        }

        // TODO: FINISH THIS F5

        #region ILoginServer Members

        public IWorld GetWorldById(int worldId)
        {
            return this.worldManager.GetWorldById(worldId);
        }

        #endregion

        private void HandleAccept(Socket socket)
        {
            NetworkSession networkSession = NetworkSession.New(socket);
            var newClient = new LoginClient(networkSession, this);
            this.clients.Add(newClient);
        }
    }
}