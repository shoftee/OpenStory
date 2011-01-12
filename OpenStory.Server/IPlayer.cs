namespace OpenStory.Server
{
    /// <summary>
    /// Provides methods for accessing Player information.
    /// </summary>
    public interface IPlayer
    {
        int CharacterId { get; }
        string CharacterName { get; }
        int ChannelId { get; }
        int Level { get; }
        int JobId { get; }
        int MapId { get; }
    }
}