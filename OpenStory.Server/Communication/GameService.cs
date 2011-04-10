using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Lifetime;

namespace OpenStory.Server.Communication
{
    /// <summary>
    /// Base class for Game services.
    /// </summary>
    public abstract class GameService : MarshalByRefObject
    {
        /// <summary>
        /// Overridden to return null, GameServices are eternal!
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
