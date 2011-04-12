using System;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a base class for cryptographic packet transformation.
    /// </summary>
    public abstract class AbstractCrypto
    {
        private readonly AesTransform encryptor;
        private readonly AesTransform decryptor;

        /// <summary>
        /// Initalizes the <see cref="Encryptor"/> and <see cref="Decryptor"/> properties.
        /// </summary>
        /// <param name="encryptor">The encryption transformer.</param>
        /// <param name="decryptor">The decryption transformer.</param>
        protected AbstractCrypto(AesTransform encryptor, AesTransform decryptor)
        {
            this.encryptor = encryptor;
            this.decryptor = decryptor;
        }

        /// <summary>
        /// Gets the <see cref="AesTransform"/> object used for the encryption transformations.
        /// </summary>
        protected AesTransform Encryptor
        {
            get { return this.encryptor; }
        }

        /// <summary>
        /// Gets the <see cref="AesTransform"/> object used for the decryption transformations.
        /// </summary>
        protected AesTransform Decryptor
        {
            get { return this.decryptor; }
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
        /// <returns>An array with the encrypted packet and its header.</returns>
        public byte[] EncryptAndPack(byte[] packetData)
        {
            if (packetData == null) throw new ArgumentNullException("packetData");
            int length = packetData.Length;
            var rawData = new byte[length + 4];
            lock (this.Encryptor)
            {
                byte[] header = this.ConstructHeader(length);
                Buffer.BlockCopy(header, 0, rawData, 0, 4);

                CustomEncryption.Encrypt(packetData);
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
            this.Decryptor.Transform(packet);
            CustomEncryption.Decrypt(packet);
        }

        /// <summary>
        /// Attempts to unpack the packet data from the given array and decrypt it.
        /// </summary>
        /// <remarks>
        /// This method will return false if and only if the header was invalid.
        /// </remarks>
        /// <param name="rawData">The raw packet data.</param>
        /// <param name="decryptedData">A reference to hold the decrypted data.</param>
        /// <returns>true if the operation was successful; if the header was invalid, false.</returns>
        public bool TryUnpackAndDecrypt(byte[] rawData, out byte[] decryptedData)
        {
            int length = this.TryGetLength(rawData);
            if (length == -1) goto Fail;

            decryptedData = ByteHelpers.SegmentFrom(rawData, 4, length);
            this.Decrypt(decryptedData);
            return true;

        Fail:
            decryptedData = null;
            return false;
        }

        /// <summary>
        /// Constructs a packet header and encodes the given length in it.
        /// </summary>
        /// <remarks>
        /// When overriding this method in a derived class, 
        /// do not call the base implementation.
        /// </remarks>
        /// <param name="length">The length of the packet.</param>
        /// <returns>A 4-element byte array which is prepended to the packet data.</returns>
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
        /// <returns>If the header is valid, the length of the packet; otherwise, -1.</returns>
        public virtual int TryGetLength(byte[] header)
        {
            bool isValid = this.Decryptor.ValidateHeader(header);
            if (!isValid)
            {
                return -1;
            }
            else
            {
                return AesTransform.GetPacketLength(header);
            }
        }
    }
}