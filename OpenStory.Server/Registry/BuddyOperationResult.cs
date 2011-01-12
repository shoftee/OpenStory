namespace OpenStory.Server.Registry
{
    public enum BuddyOperationResult : byte
    {
        None = 0,
        BuddyListFull = 11,
        TargetBuddyListFull = 12,
        AlreadyOnList = 13,
        CharacterNotFound = 15,
        Success
    }
}