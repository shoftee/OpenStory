namespace OpenStory.Cryptography
{
    /// <summary>
    /// Denotes the type of the version representation.
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// The version is used as-is.
        /// </summary>
        Regular = 0,
        /// <summary>
        /// The one's complement of the version is used.
        /// </summary>
        Complement = 1
      }
 }