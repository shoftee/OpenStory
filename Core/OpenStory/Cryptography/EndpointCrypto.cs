using System;
using OpenStory.Common;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a base class for cryptographic packet transformation.
    /// </summary>
    public sealed class EndpointCrypto
    {
        private readonly RollingIv encryptor;
        private readonly RollingIv decryptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointCrypto"/> class.
        /// </summary>
        /// <param name="encryptor">The IV used for encryption.</param>
        /// <param name="decryptor">The IV used for decryption.</param>
        private EndpointCrypto(RollingIv encryptor, RollingIv decryptor)
        {
            this.encryptor = encryptor;
            this.decryptor = decryptor;
        }

        /// <summary>
        /// Encrypts a packet, constructs a header for it and packs them into a new array.
        /// </summary>
        /// <remarks>
        /// The array given as the <paramref name="packetData"/> parameter is transformed in-place.
        /// </remarks>
        /// <param name="packetData">The packet data to encrypt and pack.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="packetData"/> is <see langword="null"/>.
        /// </exception>
        /// <returns>an array with the encrypted packet and its header.</returns>
        public byte[] EncryptAndPack(byte[] packetData)
        {
            Guard.NotNull(() => packetData, packetData);

            int length = packetData.Length;
            var rawData = new byte[length + 4];
            lock (this.encryptor)
            {
                byte[] header = this.ConstructHeader(length);
                Buffer.BlockCopy(header, 0, rawData, 0, 4);

                CustomCrypto.Encrypt(packetData);
                this.encryptor.Transform(packetData);
            }

            Buffer.BlockCopy(packetData, 0, rawData, 4, length);
            return rawData;
        }

        /// <summary>
        /// Decrypts the given packet in-place.
        /// </summary>
        /// <remarks>
        /// The array will be modified directly.
        /// </remarks>
        /// <param name="packet">The data to decrypt.</param>
        public void Decrypt(byte[] packet)
        {
            lock (this.decryptor)
            {
                this.decryptor.Transform(packet);
                CustomCrypto.Decrypt(packet);
            }
        }

        /// <summary>
        /// Attempts to unpack the packet data from the given array and decrypt it.
        /// </summary>
        /// <remarks>
        /// This method will return <see langword="false"/> if and only if the header was invalid.
        /// </remarks>
        /// <param name="rawData">The raw packet data.</param>
        /// <param name="decryptedData">A reference to hold the decrypted data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rawData"/> is <see langword="null"/>.</exception>
        /// <returns><see langword="true"/> if the operation was successful; if the header was invalid, <see langword="false"/>.</returns>
        public bool TryUnpackAndDecrypt(byte[] rawData, out byte[] decryptedData)
        {
            Guard.NotNull(() => rawData, rawData);

            int length;
            if (this.TryGetLength(rawData, out length))
            {
                decryptedData = rawData.CopySegment(4, length);
                this.Decrypt(decryptedData);
                return true;
            }
            else
            {
                decryptedData = default(byte[]);
                return false;
            }
        }

        /// <summary>
        /// Constructs a packet header and encodes the given length in it.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class,
        /// do not call the base implementation.
        /// </remarks>
        /// <param name="length">The length of the packet.</param>
        /// <returns>a 4-element byte array which should be prepended to the packet data.</returns>
        private byte[] ConstructHeader(int length)
        {
            return this.encryptor.ConstructHeader(length);
        }

        /// <summary>
        /// Attempts to extract the length of a packet from its header.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class,
        /// do not call the base implementation.
        /// </remarks>
        /// <param name="header">The header byte array to process.</param>
        /// <param name="length">A variable to hold the result.</param>
        /// <returns><see langword="true"/> if the extraction was successful; otherwise, <see langword="false"/>.</returns>
        public bool TryGetLength(byte[] header, out int length)
        {
            if (this.decryptor.ValidateHeader(header))
            {
                length = RollingIv.GetPacketLength(header);
                return true;
            }
            else
            {
                length = default(int);
                return false;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EndpointCrypto"/> class used for client-side cryptography.
        /// </summary>
        /// <remarks>
        /// Encryption uses the local IV, decryption uses the remote IV.
        /// Server's local IV has flipped version, Client's local IV has regular version.
        /// </remarks>
        /// <param name="factory">The <see cref="RollingIvFactory"/> instance to use.</param>
        /// <param name="clientIv">The IV for the client.</param>
        /// <param name="serverIv">The IV for the server.</param>
        /// <returns>a new <see cref="EndpointCrypto"/> instance.</returns>
        public static EndpointCrypto Client(RollingIvFactory factory, byte[] clientIv, byte[] serverIv)
        {
            Guard.NotNull(() => factory, factory);
            Guard.NotNull(() => clientIv, clientIv);
            Guard.NotNull(() => serverIv, serverIv);

            var encryptor = factory.CreateEncryptIv(clientIv, VersionMaskType.None);
            var decryptor = factory.CreateDecryptIv(serverIv, VersionMaskType.Complement);

            return new EndpointCrypto(encryptor, decryptor);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EndpointCrypto"/> class used for server-side cryptography.
        /// </summary>
        /// <remarks>
        /// Encryption uses the local IV, decryption uses the remote IV.
        /// Server's local IV has flipped version, Client's local IV has regular version.
        /// </remarks>
        /// <param name="factory">The <see cref="RollingIvFactory"/> instance to use.</param>
        /// <param name="clientIv">The IV for the client.</param>
        /// <param name="serverIv">The IV for the server.</param>
        /// <returns>a new <see cref="EndpointCrypto"/> instance.</returns>
        public static EndpointCrypto Server(RollingIvFactory factory, byte[] clientIv, byte[] serverIv)
        {
            Guard.NotNull(() => factory, factory);
            Guard.NotNull(() => clientIv, clientIv);
            Guard.NotNull(() => serverIv, serverIv);

            var encryptor = factory.CreateEncryptIv(clientIv, VersionMaskType.Complement);
            var decryptor = factory.CreateDecryptIv(serverIv, VersionMaskType.None);

            return new EndpointCrypto(encryptor, decryptor);
        }
    }
}
