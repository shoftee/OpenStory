namespace OpenStory.Server.Registry
{
    internal enum GuildOperationResult : byte
    {
        Unknown = 0,
        AlreadyInGuild = 0x28,
        NotInChannel = 0x2A,
        NotInGuild = 0x2D
    }
}