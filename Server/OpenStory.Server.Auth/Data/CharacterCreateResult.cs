namespace OpenStory.Server.Auth.Data
{
    /// <summary>
    /// The possible results for a character creation operation.
    /// </summary>
    public enum CharacterCreateResult
    {
        /// <summary>
        /// The character was successfully created.
        /// </summary>
        Success,

        /// <summary>
        /// The name was unavailable.
        /// </summary>
        NameUnavailable,

        /// <summary>
        /// Some other unknown error?
        /// </summary>
        Error,
    }
}
