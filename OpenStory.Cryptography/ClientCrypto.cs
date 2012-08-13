namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a cryptographic transformer for client-side sessions.
    /// </summary>
    public sealed class ClientCrypto : AbstractCrypto
    {
        // Encryption uses the local IV, decryption uses the remote IV.
        // Server's local IV has flipped version, Client's local IV has regular version.

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCrypto"/> class.
        /// </summary>
        /// <param name="factory">The <see cref="AesTransformFactory"/> to use to create <see cref="AesTransform"/> instances.</param>
        /// <param name="clientIv">The client's IV.</param>
        /// <param name="serverIv">The server's IV.</param>
        public ClientCrypto(AesTransformFactory factory, byte[] clientIv, byte[] serverIv)
            :
                base(
                encryptor: factory.Create(clientIv, VersionType.Regular),
                decryptor: factory.Create(serverIv, VersionType.Complement)) { }
    }
}