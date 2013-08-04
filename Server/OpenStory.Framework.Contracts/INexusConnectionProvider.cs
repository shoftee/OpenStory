namespace OpenStory.Framework.Contracts
{
    /// <summary>
    /// Provides methods for creating <see cref="NexusConnection"/> objects.
    /// </summary>
    public interface INexusConnectionProvider
    {
        /// <summary>
        /// Gets a <see cref="NexusConnection"/> object.
        /// </summary>
        NexusConnection GetConnection();
    }
}