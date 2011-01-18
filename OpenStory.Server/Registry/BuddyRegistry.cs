using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Server.Registry
{
    sealed class BuddyRegistry
    {
        private static readonly BuddyRegistry Instance;
        static BuddyRegistry()
        {
            Instance = new BuddyRegistry();
        }

        private BuddyRegistry()
        {
            
        }
    }
}
