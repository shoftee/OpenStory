using OpenStory.AuthService;
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

        private UniverseManager universeManager;

        public Emulator()
        {
            this.universeManager = new UniverseManager();
        }

        /// <summary>
        /// Initializes the Emulator.
        /// </summary>
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
            
            this.universeManager.Initialize();
        }
    }
}