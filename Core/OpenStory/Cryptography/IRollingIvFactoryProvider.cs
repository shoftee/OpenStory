namespace OpenStory.Cryptography
{
    /// <summary>
    /// Provides <see cref="RollingIvFactory"/> instances.
    /// </summary>
    public interface IRollingIvFactoryProvider
    {
        /// <summary>
        /// Gets a new <see cref="RollingIvFactory"/>.
        /// </summary>
        /// <param name="version">The version to use for the factory.</param>
        RollingIvFactory CreateFactory(ushort version);
    }
}