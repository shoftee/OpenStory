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
        /// <param name="clientIV">The client's IV.</param>
        /// <param name="serverIV">The server's IV.</param>
        /// <param name="version">The version of the server.</param>
        public ServerCrypto(byte[] clientIV, byte[] serverIV, ushort version) :
            base(
            encryptor: new AesTransform(serverIV, version, VersionType.Complement),
            decryptor: new AesTransform(clientIV, version, VersionType.Regular)) 
        { }
    }
}