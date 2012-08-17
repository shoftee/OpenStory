namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a cryptographic transformer for server-side sessions.
    /// </summary>
    public sealed class ServerCrypto : AbstractCrypto
    {
        // Encryption uses the local IV, decryption uses the remote IV.
        // Server's local IV has flipped version, Client's local IV has regular version.

        /// <summary>
        /// Creates a new instance of <see cref="ServerCrypto"/>.
        /// </summary>
        /// <param name="factory">The <see cref="RollingIvFactory"/> instance to use.</param>
        /// <param name="clientIv">The IV for the client.</param>
        /// <param name="serverIv">The IV for the server.</param>
        /// <returns></returns>
        public static AbstractCrypto New(RollingIvFactory factory, byte[] clientIv, byte[] serverIv)
        {
            var encryptor = factory.CreateEncryptIv(clientIv, VersionMaskType.Complement);
            var decryptor = factory.CreateDecryptIv(serverIv, VersionMaskType.None);

            return new ServerCrypto(encryptor, decryptor);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCrypto"/> class.
        /// </summary>
        /// <inheritdoc/>
        private ServerCrypto(RollingIv encryptor, RollingIv decryptor)
            : base(encryptor, decryptor)
        {
        }
    }
}