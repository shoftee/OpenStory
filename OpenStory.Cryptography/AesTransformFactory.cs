namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a class used to create <see cref="AesTransform"/> instances.
    /// </summary>
    public sealed class AesTransformFactory
    {
        private readonly CryptoTransform transform;

        /// <summary>
        /// Gets the version for the AES transformations.
        /// </summary>
        public ushort Version { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="AesTransformFactory"/>.
        /// </summary>
        /// <param name="transform">The <see cref="CryptoTransform"/> instance to use.</param>
        /// <param name="version">The version number to assign to created <see cref="AesTransform"/> instances.</param>
        public AesTransformFactory(CryptoTransform transform, ushort version)
        {
            this.transform = transform;
            this.Version = version;
        }

        /// <summary>
        /// Creates a new <see cref="AesTransform"/> instance.
        /// </summary>
        /// <param name="iv">The IV for the new instance.</param>
        /// <param name="versionType">The <see cref="VersionType"/> for the new instance.</param>
        /// <returns>a new instance of <see cref="AesTransform"/>.</returns>
        public AesTransform Create(byte[] iv, VersionType versionType)
        {
            return new AesTransform(this.transform, iv, this.Version, versionType);
        }
    }
}
