namespace OpenStory.Common.Auth
{
    /// <summary>
    /// Provides properties for game channels.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Gets the ID number of the channel.
        /// </summary>
        byte Id { get; }

        /// <summary>
        /// Gets the ID of the world hosting the channel.
        /// </summary>
        byte WorldId { get; }

        /// <summary>
        /// Gets the name of the channel.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a number between 0 and 1200 denoting how populated the channel is.
        /// </summary>
        int ChannelLoad { get; }
    }
}