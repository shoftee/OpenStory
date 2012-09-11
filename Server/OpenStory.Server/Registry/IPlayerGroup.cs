namespace OpenStory.Server.Registry
{
    /// <summary>
    /// Provides a base interface for player groups.
    /// </summary>
    public interface IPlayerGroup
    {
        /// <summary>
        /// Gets the 32-bit identifier for the group.
        /// </summary>
        int Id { get; }
    }
}
