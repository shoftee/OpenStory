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

        private WorldManager worldManager;
        private Acceptor acceptor;

        private List<LoginClient> clients;

        private LoginServer()
        {
            this.worldManager = new WorldManager();
            this.acceptor = new Acceptor(Port, this.AcceptCallback);
            this.clients = new List<LoginClient>();
        }

        // TODO: FINISH THIS F5
        private void AcceptCallback(Socket socket)
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

    public interface ILoginServer
    {
        IWorld GetWorldById(int worldId);
    }
}
