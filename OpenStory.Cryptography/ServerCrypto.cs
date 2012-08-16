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
        /// Initializes a new instance of the <see cref="ServerCrypto"/> class.
        /// </summary>
        /// <param name="factory">The <see cref="RollingIvFactory"/> to use to create <see cref="RollingIv"/> instances.</param>
        /// <param name="clientIv">The client's IV.</param>
        /// <param name="serverIv">The server's IV.</param>
        public ServerCrypto(RollingIvFactory factory, byte[] clientIv, byte[] serverIv)
            : base(
                factory.CreateEncryptIv(serverIv, VersionMaskType.Complement),
                factory.CreateDecryptIv(clientIv, VersionMaskType.None)) { }
    }
}