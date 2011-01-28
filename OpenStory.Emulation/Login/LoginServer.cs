using System.Collections.Generic;
using System.Linq;
using OpenStory.Networking;
using OpenStory.Server;
using OpenStory.Server.Login;

namespace OpenStory.Emulation.Login
{
    /// <summary>
    /// Represents a server that handles the log-on process.
    /// </summary>
    sealed class LoginServer : AbstractServer, ILoginServer
    {
        // TODO: Loading from property file?... or DB?
        private const int Port = 8484;

        /// <summary>
        /// Initial maximum number of characters on a single account.
        /// </summary>
        public const int MaxCharacters = 3;

        /// <summary>
        /// The name of the server.
        /// </summary>
        public const string ServerName = "OpenStory";

        private readonly List<LoginClient> clients;
        private readonly List<World> worlds;

        /// <summary>
        /// Initializes a new instance of the LoginServer class.
        /// </summary>
        public LoginServer()
            : base(Port)
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
            base.ThrowIfNotRunning();
            return this.worlds.First(w => w.Id == worldId);
        }

        #endregion

        protected override void HandleSession(ServerSession serverSession)
        {
            LoginClient newClient = new LoginClient(serverSession, this);
            this.clients.Add(newClient);
        }

    }
}