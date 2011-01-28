using System;
using System.Collections.Generic;
using System.Linq;
using OpenStory.Common.Tools;

namespace OpenStory.Emulation
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

        private WorldDomainManager worldManager;
        private LoginDomain loginDomain;

        /// <summary>
        /// Initializes the Emulator.
        /// </summary>
        public Emulator()
        {
            this.worldManager = new WorldDomainManager();
            this.loginDomain = new LoginDomain();
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
            loginDomain.StartLoginServer();
        }
    }
}