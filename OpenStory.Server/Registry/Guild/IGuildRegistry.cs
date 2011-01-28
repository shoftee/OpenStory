namespace OpenStory.Server.Registry.Guild
{
    /// <summary>
    /// Provides methods for creation and access of guilds.
    /// </summary>
    public interface IGuildRegistry
    {
        /// <summary>
        /// Creates a new guild with the given character as its leader.
        /// </summary>
        /// <param name="master">The character which will be the leader of the new guild.</param>
        /// <param name="guildName">The name of the new guild.</param>
        /// <returns>An <see cref="IGuild"/> object representing the new guild.</returns>
        IGuild CreateGuild(IPlayer master, string guildName);
    }
}