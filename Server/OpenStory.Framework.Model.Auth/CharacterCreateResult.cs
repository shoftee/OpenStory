namespace OpenStory.Framework.Model.Auth
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
    }
}
