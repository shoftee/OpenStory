namespace OpenStory.Cryptography
{
    /// <summary>
    /// Denotes the type of the version representation.
    /// </summary>
    public enum VersionMaskType
    {
        /// <summary>
        /// The version is used as-is.
        /// </summary>
        None = 0,

        /// <summary>
        /// The one's complement of the version is used.
        /// </summary>
        Complement = 1
    }
}
