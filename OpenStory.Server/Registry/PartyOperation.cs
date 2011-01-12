namespace OpenStory.Server.Registry
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