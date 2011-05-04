using System.Collections.Generic;

namespace OpenStory.Common.Authentication
{
    /// <summary>
    /// Provides properties of a game World.
    /// </summary>
    public interface IWorld
    {
        /// <summary>
        /// Gets the internal ID of the World.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets the name of the World.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="ServerStatus"/> for the World.
        /// </summary>
        ServerStatus Status { get; }

        /// <summary>
        /// Gets the number of channels in the World.
        /// </summary>
        int ChannelCount { get; }

        /// <summary>
        /// Gets an enumerable list of channels for the World.
        /// </summary>
        IEnumerable<IChannel> Channels { get; }
    }
}