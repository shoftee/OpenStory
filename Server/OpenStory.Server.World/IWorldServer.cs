namespace OpenStory.Server.World
{
    /// <summary>
    /// Provides methods for operating with a World Server.
    /// </summary>
    internal interface IWorldServer
    {
        /// <summary>
        /// Gets the World-to-Channel link object for the specified channel ID.
        /// </summary>
        /// <param name="channelId">The ID of the channel.</param>
        /// <returns>a <see cref="IWorldChannel"/> instance.</returns>
        IWorldChannel GetChannelById(int channelId);
    }
}
