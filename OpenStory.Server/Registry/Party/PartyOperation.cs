namespace OpenStory.Server.Registry.Party
{
    internal enum PartyOperation
    {
        None = 0,
        Join,
        Leave,
        Expel,
        Disband,
        SilentUpdate,
        ChangeLeader
    }
}