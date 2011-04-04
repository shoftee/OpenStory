namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents an unpacker for packets.
    /// </summary>
    public sealed class Unpacker
    {
        /// <summary>
        /// The internal AES transform object for this Unpacker.
        /// </summary>
        private AesTransform aesTransform;

        /// <summary>
        /// Gets a copy of the current IV of the internal AES transform object.
        /// </summary>
        public byte[] IV { get { return aesTransform.IV; } }

        /// <summary>
        /// Initializes a new instance of the Unpacker class.
        /// </summary>
        /// <param name="iv">The IV for the internal AES transformation.</param>
        /// <param name="version">The game version.</param>
        /// <param name="versionType">The internal representation of the game version.</param>
        public Unpacker(byte[] iv, ushort version, VersionType versionType)
        {
            this.aesTransform = new AesTransform(iv, version, versionType);
        }

        /// <summary>Decrypts raw packet data in-place.</summary>
        /// <param name="packetRawData">The raw data to decrypt.</param>
        public void Decrypt(byte[] packetRawData)
        {
            this.aesTransform.Transform(packetRawData);
            CustomEncryption.Decrypt(packetRawData);
        }

        /// <summary>
        /// Checks the validity of a header and 
        /// extracts the packet length from it.
        /// </summary>
        /// <param name="header">The byte array to check.</param>
        /// <returns>if the header is not valid, -1; otherwise, the packet length.</returns>
        public int CheckHeaderAndGetLength(byte[] header)
        {
            return this.aesTransform.CheckHeaderAndGetLength(header);
        }

        /// <summary>
        /// Takes a raw encrypted packet starting with a 4-byte 
        /// header, and attempts to validate and decrypt it.
        /// </summary>
        /// <remarks>
        /// The <paramref name="rawData"/> array will not be modified.
        /// </remarks>
        /// <param name="rawData">The data to unpack and decrypt.</param>
        /// <param name="decryptedData">An array reference to hold the decrypted data.</param>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        public bool TryUnpackAndDecrypt(byte[] rawData, out byte[] decryptedData)
        {
            int length = this.CheckHeaderAndGetLength(rawData);
            if (length == -1) goto Fail;

            decryptedData = ByteHelpers.SegmentFrom(rawData, 4, length);
            this.Decrypt(decryptedData);
            return true;

        Fail:
            decryptedData = null;
            return false;
        }
    }
}
