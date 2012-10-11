namespace OpenStory.Server.Auth.Data
{
    /// <summary>
    /// The possible results of a character removal operation.
    /// </summary>
    public enum CharacterRemoveResult
    {
        /// <summary>
        /// The character was successfully removed.
        /// </summary>
        Success,

        /// <summary>
        /// The character is the leader of a guild, so it cannot be removed.
        /// </summary>
        IsGuildLeader,

        /// <summary>
        /// Some other unknown error?
        /// </summary>
        Error,
    }
}