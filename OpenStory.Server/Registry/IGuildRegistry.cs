namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides methods for creation and access of guilds.
    /// </summary>
    public interface IGuildRegistry
    {
        IGuild CreateGuild(IPlayer master, string guildName);
    }
}