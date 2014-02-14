namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a class used to create <see cref="RollingIv"/> instances.
    /// </summary>
    public sealed class RollingIvFactory
    {
        private readonly ICryptoAlgorithm encryptionAlgorithm;
        private readonly ICryptoAlgorithm decryptionAlgorithm;
        private readonly ushort version;

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingIvFactory"/> class.
        /// </summary>
        /// <param name="symmetricAlgorithm">The <see cref="ICryptoAlgorithm"/> instance to use for both encryption and decryption.</param>
        /// <param name="version">The version number to assign to created <see cref="RollingIv"/> instances.</param>
        public RollingIvFactory(ICryptoAlgorithm symmetricAlgorithm, ushort version)
        {
            this.encryptionAlgorithm = symmetricAlgorithm;
            this.decryptionAlgorithm = symmetricAlgorithm;

            this.version = version;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingIvFactory"/> class.
        /// </summary>
        /// <param name="encryptionAlgorithm">The <see cref="ICryptoAlgorithm"/> instance to use for encryption IVs.</param>
        /// <param name="decryptionAlgorithm">The <see cref="ICryptoAlgorithm"/> instance to use for decryption IVs.</param>
        /// <param name="version">The version number to assign to created <see cref="RollingIv"/> instances.</param>
        public RollingIvFactory(ICryptoAlgorithm encryptionAlgorithm, ICryptoAlgorithm decryptionAlgorithm, ushort version)
        {
            this.encryptionAlgorithm = encryptionAlgorithm;
            this.decryptionAlgorithm = decryptionAlgorithm;

            this.version = version;
        }

        /// <summary>
        /// Creates a new <see cref="RollingIv"/> instance.
        /// </summary>
        /// <param name="initialIv">The initial IV for the new instance.</param>
        /// <param name="versionMaskType">The <see cref="VersionMaskType"/> for the new instance.</param>
        /// <returns>a new instance of <see cref="RollingIv"/>.</returns>
        public RollingIv CreateEncryptIv(byte[] initialIv, VersionMaskType versionMaskType)
        {
            ushort versionMask = GetMaskedVersion(this.version, versionMaskType);
            return new RollingIv(this.encryptionAlgorithm, initialIv, versionMask);
        }

        /// <summary>
        /// Creates a new <see cref="RollingIv"/> instance.
        /// </summary>
        /// <param name="initialIv">The initial IV for the new instance.</param>
        /// <param name="versionMaskType">The <see cref="VersionMaskType"/> for the new instance.</param>
        /// <returns>a new instance of <see cref="RollingIv"/>.</returns>
        public RollingIv CreateDecryptIv(byte[] initialIv, VersionMaskType versionMaskType)
        {
            ushort versionMask = GetMaskedVersion(this.version, versionMaskType);
            return new RollingIv(this.decryptionAlgorithm, initialIv, versionMask);
        }

        private static ushort GetMaskedVersion(ushort version, VersionMaskType versionMaskType)
        {
            if (versionMaskType == VersionMaskType.None)
            {
                return version;
            }
            else
            {
                return (ushort)(0xFFFF - version);
            }
        }
    }
}
