namespace OpenStory.Server.Auth.Data
{
    /// <summary>
    /// A list of possible results for the creation of a character.
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
        /// Some other error?
        /// </summary>
        Error
    }
}