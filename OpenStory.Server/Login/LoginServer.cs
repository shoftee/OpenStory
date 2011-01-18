using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using OpenStory.Server.Networking;
using OpenStory.Server.Synchronization;

namespace OpenStory.Server.Login
{
    /// <summary>
    /// Represents a server that handles the log-on process.
    /// </summary>
    public sealed class LoginServer : AbstractServer, ILoginServer
    {

        private static readonly LoginServer InternalInstance;
        private static readonly ISynchronized<LoginServer> SynchronizedInstance;
        static LoginServer()
        {
            InternalInstance = new LoginServer();
            SynchronizedInstance = Synchronizer.Synchronize(InternalInstance);
        }

        /// <summary>
        /// Synchronized entry point for the LoginServer.
        /// </summary>
        public static ISynchronized<ILoginServer> Instance
        {
            get { return SynchronizedInstance; }
        }

        // TODO: Loading from property file?... or DB?
        private const int Port = 8484;

        /// <summary>
        /// Initial maximum number of characters on a single account.
        /// </summary>
        public const int MaxCharacters = 3;

        /// <summary>
        /// The name of the server.
        /// </summary>
        public const string ServerName = "OpenMS";

        private readonly List<LoginClient> clients;
        private readonly List<World> worlds;

        private LoginServer() : base(Port)
        {
            this.worlds = new List<World>();
            this.clients = new List<LoginClient>();
        }

        // TODO: FINISH THIS F5

        #region ILoginServer Members

        /// <summary>
        /// Gets a <see cref="IWorld"/> instance by the World's ID.
        /// </summary>
        /// <param name="worldId">The ID of the world.</param>
        /// <returns>An <see cref="IWorld"/> object which represents the world with the given ID.</returns>
        public IWorld GetWorldById(int worldId)
        {
            return this.worlds.First(w => w.Id == worldId);
        }

        #endregion

        /// <summary>
        /// This method is called when a socket connection is accepted.
        /// </summary>
        /// <param name="socket">The socket for the new connection.</param>
        protected override void HandleAccept(Socket socket)
        {
            NetworkSession networkSession = NetworkSession.New(socket);
            var newClient = new LoginClient(networkSession, this);
            this.clients.Add(newClient);
        }
    }
}