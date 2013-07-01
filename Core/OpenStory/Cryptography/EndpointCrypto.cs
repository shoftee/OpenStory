using System;
using OpenStory.Common.Tools;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a base class for cryptographic packet transformation.
    /// </summary>
    public abstract class EndpointCrypto
    {
        private readonly RollingIv encryptor;
        private readonly RollingIv decryptor;

        /// <summary>
        /// Gets the <see cref="RollingIv"/> object used for the encryption transformations.
        /// </summary>
        protected RollingIv Encryptor
        {
            get { return this.encryptor; }
        }

        /// <summary>
        /// Gets the <see cref="RollingIv"/> object used for the decryption transformations.
        /// </summary>
        protected RollingIv Decryptor
        {
            get { return this.decryptor; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndpointCrypto"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the <see cref="Encryptor"/> and <see cref="Decryptor"/> properties.
        /// </remarks>
        /// <param name="encryptor">The IV used for encryption.</param>
        /// <param name="decryptor">The IV used for decryption.</param>
        protected EndpointCrypto(RollingIv encryptor, RollingIv decryptor)
        {
            if (encryptor == null)
            {
                throw new ArgumentNullException("encryptor");
            }

            if (decryptor == null)
            {
                throw new ArgumentNullException("decryptor");
            }

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
        /// Thrown if <paramref name="packetData"/> is <c>null</c>.
        /// </exception>
        /// <returns>an array with the encrypted packet and its header.</returns>
        public byte[] EncryptAndPack(byte[] packetData)
        {
            if (packetData == null)
            {
                throw new ArgumentNullException("packetData");
            }

            int length = packetData.Length;
            var rawData = new byte[length + 4];
            lock (this.Encryptor)
            {
                byte[] header = this.ConstructHeader(length);
                Buffer.BlockCopy(header, 0, rawData, 0, 4);

                CustomCrypto.Encrypt(packetData);
                this.Encryptor.Transform(packetData);
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
            lock (this.Decryptor)
            {
                this.Decryptor.Transform(packet);
                CustomCrypto.Decrypt(packet);
            }
        }

        /// <summary>
        /// Attempts to unpack the packet data from the given array and decrypt it.
        /// </summary>
        /// <remarks>
        /// This method will return <c>false</c> if and only if the header was invalid.
        /// </remarks>
        /// <param name="rawData">The raw packet data.</param>
        /// <param name="decryptedData">A reference to hold the decrypted data.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rawData"/> is <c>null</c>.</exception>
        /// <returns><c>true</c> if the operation was successful; if the header was invalid, <c>false</c>.</returns>
        public bool TryUnpackAndDecrypt(byte[] rawData, out byte[] decryptedData)
        {
            if (rawData == null)
            {
                throw new ArgumentNullException("rawData");
            }

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
        protected virtual byte[] ConstructHeader(int length)
        {
            return this.Encryptor.ConstructHeader(length);
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
        /// <returns><c>true</c> if the extraction was successful; otherwise, <c>false</c>.</returns>
        public virtual bool TryGetLength(byte[] header, out int length)
        {
            if (this.Decryptor.ValidateHeader(header))
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
    }
}
