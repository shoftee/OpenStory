using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStory.Common.Data;
using OpenStory.Common.IO;

namespace OpenStory.Server
{
    /// <summary>
    /// Provides methods for common game server operations.
    /// </summary>
    public interface IGameServer
    {
        /// <summary>
        /// Creates a new <see cref="PacketBuilder"/> for the packet type with the specified label.
        /// </summary>
        /// <param name="label">The label for the packet type.</param>
        /// <returns>an instance of <see cref="PacketBuilder"/>.</returns>
        PacketBuilder NewPacket(string label);
    }
}
