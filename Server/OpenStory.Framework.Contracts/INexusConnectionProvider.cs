namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for creating <see cref="NexusConnectionInfo"/> objects.
    /// </summary>
    public interface INexusConnectionProvider
    {
        /// <summary>
        /// Gets a <see cref="NexusConnectionInfo"/> object.
        /// </summary>
        NexusConnectionInfo GetConnectionInfo();
    }
}