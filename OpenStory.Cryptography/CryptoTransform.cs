using System;
using System.Security.Cryptography;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Encapsulates cryptographic transformation data and routines for the rolling packet encryption and decryption.
    /// </summary>
    public sealed class CryptoTransform
    {
        private const int IvLength = 16;
        private const int BlockLength = 1460;

        private readonly byte[] table;
        private readonly byte[] initialValue;
        private readonly byte[] key;

        private readonly ICryptoTransform transformer;

        /// <summary>
        /// Initializes a new instance of <see cref="CryptoTransform"/>.
        /// </summary>
        /// <remarks>
        /// The provided arrays are copied into the <see cref="CryptoTransform"/> instance to avoid mutation.
        /// </remarks>
        /// <param name="table">The shuffle transformation table.</param>
        /// <param name="initialValue">The initial value for the shuffle transformation.</param>
        /// <param name="key">The AES key.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the provided parameters is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if any of the provided parameters has an invalid number of elements.</exception>
        public CryptoTransform(byte[] table, byte[] initialValue, byte[] key)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (initialValue == null)
            {
                throw new ArgumentNullException("initialValue");
            }
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (table.Length != 256)
            {
                throw new ArgumentException("'table' must have exactly 256 elements.", "table");
            }
            if (initialValue.Length != 4)
            {
                throw new ArgumentException("'initialValue' must have exactly 4 elements.", "initialValue");
            }
            if (key.Length != 32)
            {
                throw new ArgumentException("'key' must have exactly 32 elements.", "key");
            }

            this.table = table.FastClone();
            this.initialValue = initialValue.FastClone();
            this.key = key.FastClone();

            this.transformer = GetTransformer(this.key);
        }

        private static ICryptoTransform GetTransformer(byte[] key)
        {
            var cipher = new RijndaelManaged
            {
                Padding = PaddingMode.None,
                Mode = CipherMode.ECB,
                Key = key
            };
            using (cipher)
            {
                var transform = cipher.CreateEncryptor();
                return transform;
            }
        }

        /// <summary>
        /// Performs the shuffle operation on a specified IV.
        /// </summary>
        /// <param name="iv">The IV to shuffle.</param>
        /// <returns>the shuffled IV.</returns>
        public byte[] ShuffleIv(byte[] iv)
        {
            byte[] shuffled = initialValue.FastClone();

            for (int i = 0; i < 4; i++)
            {
                byte input = iv[i], tableInput = table[input];

                shuffled[0] += (byte)(table[shuffled[1]] - input);
                shuffled[1] -= (byte)(shuffled[2] ^ tableInput);
                shuffled[2] ^= (byte)(table[shuffled[3]] + input);
                shuffled[3] -= (byte)(shuffled[0] - tableInput);

                uint merged = (uint)(unchecked(shuffled[3] << 24) | (shuffled[2] << 16) | (shuffled[1] << 8) | shuffled[0]);
                uint shifted = (merged << 3) | (merged >> 0x1D);

                unchecked
                {
                    shuffled[0] = (byte)shifted;
                    shuffled[1] = (byte)(shifted >> 8);
                    shuffled[2] = (byte)(shifted >> 16);
                    shuffled[3] = (byte)(shifted >> 24);
                }
            }
            return shuffled;
        }

        /// <summary>
        /// Performs the AES transformation on a single block of the data.
        /// </summary>
        /// <remarks><para>
        /// The parameter <paramref name="xorBlock"/> is used only for performance 
        /// considerations, to avoid instantiating a new array every time a transformation has 
        /// to be done. It should not be shorter than 16 elements, and it's unnecessary for 
        /// it to be longer. Its contents will be overwritten.
        /// </para></remarks>
        /// <param name="data">The array containing the block.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <param name="blockStart">The start offset of the block.</param>
        /// <param name="blockEnd">The end offset of the block.</param>
        /// <param name="xorBlock">An array to use for the internal xor operations.</param>
        private void TransformBlock(byte[] data, byte[] iv, int blockStart, int blockEnd, byte[] xorBlock)
        {
            FillXorBlock(iv, xorBlock);

            int xorBlockPosition = 0;
            for (int position = blockStart; position < blockEnd; position++)
            {
                if (xorBlockPosition == 0)
                {
                    xorBlock = this.transformer.TransformFinalBlock(xorBlock, 0, IvLength);
                }

                data[position] ^= xorBlock[xorBlockPosition];
                xorBlockPosition++;
                if (xorBlockPosition == IvLength)
                {
                    xorBlockPosition = 0;
                }
            }
        }

        /// <summary>
        /// Fills a 16-element byte array with copies of the specified IV.
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
        /// Transforms a byte array segment in-place.
        /// </summary>
        /// <remarks>
        /// The array specified used for the <paramref name="iv"/> argument will not be modified.
        /// </remarks>
        /// <param name="data">The byte containing the segment.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <param name="segmentStart">The offset of the start of the segment.</param>
        /// <param name="segmentEnd">The offset of the end of the segment.</param>
        public void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd)
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
        /// Transforms an array with a specified IV.
        /// </summary>
        /// <remarks>
        /// The specified arrays are not modified.
        /// </remarks>
        /// <param name="data">The data to transform.</param>
        /// <param name="iv">The IV to use for the transformation.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if any of the parameters are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="iv"/> does not have exactly 4 elements.
        /// </exception>
        /// <returns>A transformed copy of the array.</returns>
        public byte[] TransformWithIv(byte[] data, byte[] iv)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (iv == null)
            {
                throw new ArgumentNullException("iv");
            }
            else if (iv.Length != 4)
            {
                throw new ArgumentException("'iv' must have exactly 4 elements.");
            }

            var copy = data.FastClone();
            TransformArraySegment(copy, iv, 0, copy.Length);
            return copy;
        }
    }
}