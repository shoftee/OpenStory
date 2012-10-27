using System;
using System.Security.Cryptography;
using OpenStory.Common.Tools;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a cryptographic transformer based on the AES algorithm.
    /// </summary>
    public sealed class AesTransform : CryptoTransformBase
    {
        private const int IvLength = 16;
        private const int BlockLength = 1460;
        private readonly byte[] key;

        private readonly ICryptoTransform aes;

        private static ICryptoTransform GetTransformer(byte[] key)
        {
            var cipher = new RijndaelManaged()
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
        /// Initializes a new instance of <see cref="AesTransform"/>.
        /// </summary>
        /// <remarks>
        /// The provided arrays are copied into the <see cref="AesTransform"/> instance to avoid mutation.
        /// </remarks>
        /// <param name="table">The shuffle transformation table.</param>
        /// <param name="initialIv">The initial value for the shuffle transformation.</param>
        /// <param name="key">The AES key.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the provided parameters is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if any of the provided parameters has an invalid number of elements.</exception>
        public AesTransform(byte[] table, byte[] initialIv, byte[] key)
            : base(table, initialIv)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (key.Length != 32)
            {
                throw new ArgumentException(Exceptions.AesKeyMustBe32Bytes, "key");
            }

            this.key = key.FastClone();

            this.aes = GetTransformer(this.key);
        }

        /// <inheritdoc />
        public override void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd)
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
                    xorBlock = this.aes.TransformFinalBlock(xorBlock, 0, IvLength);
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
    }
}
