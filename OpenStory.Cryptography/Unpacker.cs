using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents an unpacker for packets.
    /// </summary>
    public sealed class Unpacker
    {
        /// <summary>
        /// Gets the AES transform object for this Unpacker.
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

        /// <summary>
        /// Decrypts raw packet data in-place.
        /// </summary>
        /// <param name="packetRawData">The raw data to decrypt.</param>
        public void Decrypt(byte[] packetRawData)
        {
            this.aesTransform.Transform(packetRawData);
            CustomEncryption.Decrypt(packetRawData);
        }

        /// <summary>
        /// Checks the validity of a header and extracts the packet length from it.
        /// </summary>
        /// <param name="header">The byte array to check.</param>
        /// <returns>if the header is not valid, -1; otherwise, the packet length.</returns>
        public int CheckHeaderAndGetLength(byte[] header)
        {
            return this.aesTransform.CheckHeaderAndGetLength(header);
        }
    }
}
