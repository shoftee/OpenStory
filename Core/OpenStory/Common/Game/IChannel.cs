namespace OpenStory.Common.Game
{
    /// <summary>
    /// Provides properties for game channels.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Gets the numeric channel identifier.
        /// </summary>
        byte ChannelId { get; }

        /// <summary>
        /// Gets the numeric world identifier.
        /// </summary>
        byte WorldId { get; }

        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a non-negative integer denoting how populated the channel is.
        /// </summary>
        int ChannelLoad { get; }
    }
}
