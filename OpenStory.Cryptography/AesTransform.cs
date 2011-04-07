using System;
using System.Security.Cryptography;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents an AES encryption transformer.
    /// </summary>
    public sealed class AesTransform
    {
        private const int IvLength = 16;

        private const int BlockLength = 1460;

        /// <summary>
        /// The transformation table for the shuffle routine.
        /// </summary>
        private static readonly byte[] ShuffleTable =
            {
                0xEC, 0x3F, 0x77, 0xA4, 0x45, 0xD0, 0x71, 0xBF, 0xB7, 0x98, 0x20, 0xFC, 0x4B, 0xE9, 0xB3, 0xE1,
                0x5C, 0x22, 0xF7, 0x0C, 0x44, 0x1B, 0x81, 0xBD, 0x63, 0x8D, 0xD4, 0xC3, 0xF2, 0x10, 0x19, 0xE0,
                0xFB, 0xA1, 0x6E, 0x66, 0xEA, 0xAE, 0xD6, 0xCE, 0x06, 0x18, 0x4E, 0xEB, 0x78, 0x95, 0xDB, 0xBA,
                0xB6, 0x42, 0x7A, 0x2A, 0x83, 0x0B, 0x54, 0x67, 0x6D, 0xE8, 0x65, 0xE7, 0x2F, 0x07, 0xF3, 0xAA,
                0x27, 0x7B, 0x85, 0xB0, 0x26, 0xFD, 0x8B, 0xA9, 0xFA, 0xBE, 0xA8, 0xD7, 0xCB, 0xCC, 0x92, 0xDA,
                0xF9, 0x93, 0x60, 0x2D, 0xDD, 0xD2, 0xA2, 0x9B, 0x39, 0x5F, 0x82, 0x21, 0x4C, 0x69, 0xF8, 0x31,
                0x87, 0xEE, 0x8E, 0xAD, 0x8C, 0x6A, 0xBC, 0xB5, 0x6B, 0x59, 0x13, 0xF1, 0x04, 0x00, 0xF6, 0x5A,
                0x35, 0x79, 0x48, 0x8F, 0x15, 0xCD, 0x97, 0x57, 0x12, 0x3E, 0x37, 0xFF, 0x9D, 0x4F, 0x51, 0xF5,
                0xA3, 0x70, 0xBB, 0x14, 0x75, 0xC2, 0xB8, 0x72, 0xC0, 0xED, 0x7D, 0x68, 0xC9, 0x2E, 0x0D, 0x62,
                0x46, 0x17, 0x11, 0x4D, 0x6C, 0xC4, 0x7E, 0x53, 0xC1, 0x25, 0xC7, 0x9A, 0x1C, 0x88, 0x58, 0x2C,
                0x89, 0xDC, 0x02, 0x64, 0x40, 0x01, 0x5D, 0x38, 0xA5, 0xE2, 0xAF, 0x55, 0xD5, 0xEF, 0x1A, 0x7C,
                0xA7, 0x5B, 0xA6, 0x6F, 0x86, 0x9F, 0x73, 0xE6, 0x0A, 0xDE, 0x2B, 0x99, 0x4A, 0x47, 0x9C, 0xDF,
                0x09, 0x76, 0x9E, 0x30, 0x0E, 0xE4, 0xB2, 0x94, 0xA0, 0x3B, 0x34, 0x1D, 0x28, 0x0F, 0x36, 0xE3,
                0x23, 0xB4, 0x03, 0xD8, 0x90, 0xC8, 0x3C, 0xFE, 0x5E, 0x32, 0x24, 0x50, 0x1F, 0x3A, 0x43, 0x8A,
                0x96, 0x41, 0x74, 0xAC, 0x52, 0x33, 0xF0, 0xD9, 0x29, 0x80, 0xB1, 0x16, 0xD3, 0xAB, 0x91, 0xB9,
                0x84, 0x7F, 0x61, 0x1E, 0xCF, 0xC5, 0xD1, 0x56, 0x3D, 0xCA, 0xF4, 0x05, 0xC6, 0xE5, 0x08, 0x49
            };

        /// <summary>
        /// The Initialization Vector for the shuffle routine.
        /// </summary>
        private static readonly byte[] ShuffleIV = { 0xF2, 0x53, 0x50, 0xC6 };

        /// <summary>
        /// The 256-bit key for the AES encryption.
        /// </summary>
        private static readonly byte[] AesKey =
            {
                0x13, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00,
                0x06, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00,
                0x1B, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00,
                0x33, 0x00, 0x00, 0x00, 0x52, 0x00, 0x00, 0x00
            };

        private static readonly ICryptoTransform Transformer = GetTransformer();
        private byte[] iv;

        /// <summary>Gets the current IV for this AesTransform object</summary>
        /// <remarks>Modifying the elements of this array will be very bad.</remarks>
        public byte[] IV
        {
            get { return this.iv; }
        }

        private ushort version;

        private static ICryptoTransform GetTransformer()
        {
            var cipher = new RijndaelManaged
                         {
                             Padding = PaddingMode.None,
                             Mode = CipherMode.ECB,
                             Key = AesKey
                         };
            using (cipher)
            {
                var transform = cipher.CreateEncryptor();
                return transform;
            }
        }

        /// <summary>Initializes a new instance of AesTransform.</summary>
        /// <param name="iv">The initialization vector for this instance.</param>
        /// <param name="version">The MapleStory version.</param>
        /// <param name="versionType">The representation of the version.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="iv"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="iv"/> has more than or less than 4 elements.</exception>
        public AesTransform(byte[] iv, ushort version, VersionType versionType)
        {
            if (iv == null) throw new ArgumentNullException("iv");
            if (!Enum.IsDefined(typeof(VersionType), versionType))
            {
                throw new ArgumentOutOfRangeException("versionType", "Argument 'versionType' has an invalid value.");
            }
            if (iv.Length != 4)
            {
                throw new ArgumentOutOfRangeException("iv", "Argument 'iv' does not have exactly 4 elements.");
            }

            this.iv = new byte[4];
            Buffer.BlockCopy(iv, 0, this.iv, 0, 4);

            if (versionType == VersionType.Complement)
            {
                version = (ushort) (0xFFFF - version);
            }

            // Flip the version.
            this.version = (ushort) ((version >> 8) | ((version & 0xFF) << 8));
        }

        /// <summary>
        /// Transforms the given data in-place.
        /// </summary>
        /// <param name="data">The array to transform. This array will be directly modified.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="data" /> is <c>null</c>.</exception>
        public void Transform(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            TransformArraySegment(data, this.iv, 0, data.Length);

            this.UpdateIV();
        }

        /// <summary>
        /// Transforms a byte array segment in-place.
        /// </summary>
        /// <remarks>
        /// The array given in the <paramref name="iv"/> parameter will not be modified.
        /// </remarks>
        /// <param name="data">The byte containing the segment.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <param name="segmentStart">The offset of the start of the segment.</param>
        /// <param name="segmentEnd">The offset of the end of the segment.</param>
        private static void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd)
        {
            var xorBlock = new byte[IvLength];

            // First block is 4 elements shorter because of the header.
            const int FirstBlockLength = BlockLength - 4;

            int blockStart = segmentStart;
            int blockEnd = Math.Min(blockStart + FirstBlockLength, segmentEnd);

            TransformBlock(data, iv, blockStart, blockEnd, xorBlock);

            blockStart += FirstBlockLength;
            while (blockStart < segmentEnd)
            {
                blockEnd = Math.Min(blockStart + BlockLength, segmentEnd);

                TransformBlock(data, iv, blockStart, blockEnd, xorBlock);

                blockStart += BlockLength;
            }
        }

        /// <summary>
        /// Performs the AES transformation on a single block of the data.
        /// </summary>
        /// <remarks><para>
        /// The parameter <paramref name="xorBlock"/> is given only for performance 
        /// considerations, to avoid instantiating a new array every time a transformation has 
        /// to be done. It should not be shorter than 16 elements, and it's unnecessary for 
        /// it to be longer. Its contents will be overwritten.
        /// </para></remarks>
        /// <param name="data">The array containing the block.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <param name="blockStart">The start offset of the block.</param>
        /// <param name="blockEnd">The end offset of the block.</param>
        /// <param name="xorBlock">An array to use for the internal xor operations.</param>
        private static void TransformBlock(byte[] data, byte[] iv, int blockStart, int blockEnd, byte[] xorBlock)
        {
            FillXorBlock(iv, xorBlock);

            int xorBlockPosition = 0;
            for (int position = blockStart; position < blockEnd; position++)
            {
                if (xorBlockPosition == 0)
                {
                    xorBlock = Transformer.TransformFinalBlock(xorBlock, 0, IvLength);
                }

                data[position] ^= xorBlock[xorBlockPosition++];

                if (xorBlockPosition == IvLength) xorBlockPosition = 0;
            }
        }

        /// <summary>
        /// Fills a 16-element byte array with copies of the given IV.
        /// </summary>
        /// <param name="iv">The IV to copy.</param>
        /// <param name="xorBlock">The block to use.</param>
        private static void FillXorBlock(byte[] iv, byte[] xorBlock)
        {
            for (int i = 0; i < IvLength; i += 4)
            {
                Buffer.BlockCopy(iv, 0, xorBlock, i, 4);
            }
        }

        /// <summary>
        /// Constructs a packet header.
        /// </summary>
        /// <param name="length">The length of the packet to make a header for.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if <paramref name="length"/> is less than 2.
        /// </exception>
        /// <returns>The 4-byte header for a packet with the given length.</returns>
        public byte[] ConstructHeader(int length)
        {
            if (length < 2)
            {
                throw new ArgumentOutOfRangeException("length", length, "The packet length must be at least 2.");
            }

            int encodedVersion = (((this.iv[2] << 8) | this.iv[3]) ^ this.version);
            int encodedLength = encodedVersion ^ (((length & 0xFF) << 8) | (length >> 8));

            return new[]
                   {
                       unchecked((byte) (encodedVersion >> 8)),
                       unchecked((byte) encodedVersion),
                       unchecked((byte) (encodedLength >> 8)),
                       unchecked((byte) encodedLength)
                   };
        }

        /// <summary>
        /// Reads a packet header from an array and extracts the packet's length.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="data"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="data"/> has less than 4 elements.
        /// </exception>
        /// <returns>The length of the packet which was extracted from the array.</returns>
        public static int GetPacketLength(byte[] data)
        {
            if (data == null) throw new ArgumentNullException("data");
            if (data.Length < 4)
            {
                throw GetSegmentTooShortException(4, "data");
            }

            return ((data[1] ^ data[3]) << 8) | (data[0] ^ data[2]);
        }

        /// <summary>
        /// Determines whether the start of an array is a valid packet header.
        /// </summary>
        /// <param name="header">The raw packet data to validate.</param>
        /// <returns>true if the header is valid; otherwise, false.</returns>
        public bool ValidateHeader(byte[] header)
        {
            ushort extractedVersion = GetVersion(header, this.iv);

            return extractedVersion == this.version;
        }

        /// <summary>
        /// Attempts to extract the packet length from a header.
        /// </summary>
        /// <param name="header">The byte array to check.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="header"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="header"/> has less than 4 elements.</exception>
        /// <returns>if the header is valid, the packet length; otherwise, -1.</returns>
        public int TryGetLength(byte[] header)
        {
            if (this.ValidateHeader(header))
            {
                return ((header[1] ^ header[3]) << 8) | (header[0] ^ header[2]);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Performs the IV shuffle on this instance of AesTransform.
        /// </summary>
        private void UpdateIV()
        {
            var newIV = new byte[4];
            Buffer.BlockCopy(ShuffleIV, 0, newIV, 0, 4);

            for (int i = 0; i < 4; i++)
            {
                byte input = this.iv[i], tableInput = ShuffleTable[input];

                newIV[0] += (byte) (ShuffleTable[newIV[1]] - input);
                newIV[1] -= (byte) (newIV[2] ^ tableInput);
                newIV[2] ^= (byte) (ShuffleTable[newIV[3]] + input);
                newIV[3] -= (byte) (newIV[0] - tableInput);

                uint merged = (uint) (unchecked(newIV[3] << 24) | (newIV[2] << 16) | (newIV[1] << 8) | newIV[0]);
                uint shifted = (merged << 3) | (merged >> 0x1D);

                newIV[0] = unchecked((byte) shifted);
                newIV[1] = unchecked((byte) (shifted >> 8));
                newIV[2] = unchecked((byte) (shifted >> 16));
                newIV[3] = unchecked((byte) (shifted >> 24));
            }
            this.iv = newIV;
        }

        private static ArgumentException GetSegmentTooShortException(int lowBound, string parameterName)
        {
            return new ArgumentException("The segment must have at least " + lowBound + " elements.", parameterName);
        }

        /// <summary>
        /// Transforms an array with a given IV.
        /// </summary>
        /// <remarks>
        /// The given arrays are not modified.
        /// </remarks>
        /// <param name="data">The data to transform.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <returns>A transformed copy of the array.</returns>
        public static byte[] TransformWithIV(byte[] data, byte[] iv)
        {
            int length = data.Length;
            byte[] transformedData = new byte[length];
            Buffer.BlockCopy(data, 0, transformedData, 0, length);
            TransformArraySegment(data, iv, 0, length);
            return transformedData;
        }

        /// <summary>
        /// Extracts a version from the header using the given IV.
        /// </summary>
        /// <param name="header">The header byte array.</param>
        /// <param name="iv">The IV to use for the decoding.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="header"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="header"/> has less than 4 elements.</exception>
        /// <returns>A number representing the version encoded into the header.</returns>
        public static ushort GetVersion(byte[] header, byte[] iv)
        {
            if (header == null) throw new ArgumentNullException("header");
            if (header.Length < 4)
            {
                throw GetSegmentTooShortException(4, "header");
            }

            ushort encodedVersion = (ushort) ((header[0] << 8) | header[1]);
            ushort xorSegment = (ushort) ((iv[2] << 8) | iv[3]);

            return (ushort) (encodedVersion ^ xorSegment);
        }
    }

    /// <summary>
    /// Denotes the type of the version representation.
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// The version is used as-is.
        /// </summary>
        Regular = 0,
        /// <summary>
        /// The two's complement of the version is used.
        /// </summary>
        Complement = 1
    }
}