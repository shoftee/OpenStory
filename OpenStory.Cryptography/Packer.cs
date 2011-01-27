using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a packer for packets.
    /// </summary>
    public sealed class Packer
    {
        /// <summary>
        /// Gets the AES transform object for this Packer.
        /// </summary>
        public AesTransform AesTransform { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Packer class.
        /// </summary>
        /// <param name="iv">The IV for the internal AES transformation.</param>
        /// <param name="version">The game version.</param>
        public Packer(byte[] iv, short version)
        {
            this.AesTransform = new AesTransform(iv, (short) (0xFFFF - version));
        }

        /// <summary>
        /// Encrypts a packet, constructs a header for it and packs them into a new array.
        /// </summary>
        /// <remarks>
        /// The array given as the <paramref name="packetData"/> parameter is transformed in-place.
        /// </remarks>
        /// <param name="packetData">The packet data to encrypt and pack.</param>
        /// <returns>An array with the encrypted packet and its header.</returns>
        public byte[] EncryptAndPack(byte[] packetData)
        {
            int length = packetData.Length;
            byte[] rawData = new byte[length + 4];
            lock (this.AesTransform)
            {
                byte[] header = this.AesTransform.ConstructHeader(length);
                Buffer.BlockCopy(header, 0, rawData, 0, 4);

                this.AesTransform.Transform(packetData);
                CustomEncryption.Encrypt(packetData);
            }
            Buffer.BlockCopy(packetData, 0, rawData, 4, length);
            return rawData;
        }
    }
}
