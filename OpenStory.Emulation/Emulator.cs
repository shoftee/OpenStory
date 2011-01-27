using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenStory.Common.Tools;
using OpenStory.Emulation.Helpers;

namespace OpenStory.Emulation
{
    /// <summary>
    /// The entry-point class for the server.
    /// </summary>
    public sealed class Emulator
    {
        /// <summary>
        /// Denotes whether the emulator is running or not.
        /// </summary>
        public bool IsRunning { get; private set; }

        private WorldDomainManager worldManager;
        
        /// <summary>
        /// Initializes the Emulator.
        /// </summary>
        public Emulator()
        {
            this.worldManager = new WorldDomainManager();
            worldManager.Initialize();
            if (!Initializer.Run())
            {
                Log.WriteError("Server startup failed.");
            }
            else
            {
                Log.WriteInfo("Startup successful.");
                this.IsRunning = true;
            }
        }
    }
}