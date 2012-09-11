using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Common.Data;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides methods for common game server operations.
    /// </summary>
    public interface IGameServer
    {
        /// <summary>
        /// Gets the op code table for this server.
        /// </summary>
        IOpCodeTable OpCodes { get; }
    }
}
