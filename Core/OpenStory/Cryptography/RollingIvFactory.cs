namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a class used to create <see cref="RollingIv"/> instances.
    /// </summary>
    public sealed class RollingIvFactory
    {
        /// <summary>
        /// Gets the <see cref="ICryptoAlgorithm"/> for the encryption transformations.
        /// </summary>
        public ICryptoAlgorithm EncryptionAlgorithm { get; private set; }

        /// <summary>
        /// Gets the <see cref="ICryptoAlgorithm"/> for the decryption transformations.
        /// </summary>
        public ICryptoAlgorithm DecryptionAlgorithm { get; private set; }

        /// <summary>
        /// Gets the version for the transformations.
        /// </summary>
        public ushort Version { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="RollingIvFactory"/>.
        /// </summary>
        /// <param name="symmetricAlgorithm">The <see cref="ICryptoAlgorithm"/> instance to use for both encryption and decryption.</param>
        /// <param name="version">The version number to assign to created <see cref="RollingIv"/> instances.</param>
        public RollingIvFactory(
            ICryptoAlgorithm symmetricAlgorithm, 
            ushort version)
        {
            this.EncryptionAlgorithm = symmetricAlgorithm;
            this.DecryptionAlgorithm = symmetricAlgorithm;

            this.Version = version;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RollingIvFactory"/>.
        /// </summary>
        /// <param name="encryptionAlgorithm">The <see cref="ICryptoAlgorithm"/> instance to use for encryption IVs.</param>
        /// <param name="decryptionAlgorithm">The <see cref="ICryptoAlgorithm"/> instance to use for decryption IVs.</param>
        /// <param name="version">The version number to assign to created <see cref="RollingIv"/> instances.</param>
        public RollingIvFactory(
            ICryptoAlgorithm encryptionAlgorithm, 
            ICryptoAlgorithm decryptionAlgorithm,
            ushort version)
        {
            this.EncryptionAlgorithm = encryptionAlgorithm;
            this.DecryptionAlgorithm = decryptionAlgorithm;

            this.Version = version;
        }

        /// <summary>
        /// Creates a new <see cref="RollingIv"/> instance.
        /// </summary>
        /// <param name="initialIv">The initial IV for the new instance.</param>
        /// <param name="versionMaskType">The <see cref="VersionMaskType"/> for the new instance.</param>
        /// <returns>a new instance of <see cref="RollingIv"/>.</returns>
        public RollingIv CreateEncryptIv(
            byte[] initialIv, 
            VersionMaskType versionMaskType)
        {
            ushort versionMask = this.Version;
            if (versionMaskType == VersionMaskType.Complement)
            {
                versionMask = (ushort)(0xFFFF - versionMask);
            }

            return new RollingIv(this.EncryptionAlgorithm, initialIv, versionMask);
        }

        /// <summary>
        /// Creates a new <see cref="RollingIv"/> instance.
        /// </summary>
        /// <param name="initialIv">The initial IV for the new instance.</param>
        /// <param name="versionMaskType">The <see cref="VersionMaskType"/> for the new instance.</param>
        /// <returns>a new instance of <see cref="RollingIv"/>.</returns>
        public RollingIv CreateDecryptIv(
            byte[] initialIv, 
            VersionMaskType versionMaskType)
        {
            ushort versionMask = this.Version;
            if (versionMaskType == VersionMaskType.Complement)
            {
                versionMask = (ushort)(0xFFFF - versionMask);
            }

            return new RollingIv(this.DecryptionAlgorithm, initialIv, versionMask);
        }
    }
}
