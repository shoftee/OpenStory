using System.Threading;
using OpenStory.Common.Tools;

namespace OpenStory.Server.Emulation
{
    /// <summary>
    /// The entry-point class for the server.
    /// </summary>
    sealed class Emulator
    {
        /// <summary>
        /// Denotes whether the emulator is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

        private UniverseManager worldManager;
        private AuthenticationServerDomain authenticationServerDomain;

        /// <summary>
        /// Initializes the Emulator.
        /// </summary>
        public Emulator()
        {
            this.worldManager = new UniverseManager();
            this.authenticationServerDomain = new AuthenticationServerDomain();
        }

        public void Start()
        {
            if (!Initializer.Run())
            {
                Log.WriteError("Server startup failed.");
            }
            else
            {
                Log.WriteInfo("Startup successful.");
                this.IsRunning = true;
            }

            worldManager.Initialize();
            this.authenticationServerDomain.StartAuthenticationServer();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}