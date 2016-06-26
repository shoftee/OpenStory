using OpenStory.Common;
using System;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Encapsulates cryptographic transformation data and routines for the rolling packet encryption and decryption.
    /// </summary>
    public abstract class CryptoTransformBase : ICryptoAlgorithm
    {
        private readonly byte[] table;
        private readonly byte[] iv;

        /// <summary>
        /// Gets the translation table used for the IV shuffle.
        /// </summary>
        protected byte[] Table
        {
            get { return this.table; }
        }

        /// <summary>
        /// Gets the initial IV used for the IV shuffle.
        /// </summary>
        protected byte[] Iv
        {
            get { return this.iv; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoTransformBase"/> class.
        /// </summary>
        /// <remarks>
        /// The provided arrays are copied into the <see cref="CryptoTransformBase"/> instance to avoid mutation.
        /// </remarks>
        /// <param name="table">The shuffle transformation table.</param>
        /// <param name="vector">The initial value for the shuffle transformation.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the provided parameters is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">Thrown if any of the provided arrays has an invalid number of elements.</exception>
        protected CryptoTransformBase(byte[] table, byte[] vector)
        {
            Guard.NotNull(() => table, table);
            Guard.NotNull(() => vector, vector);

            if (table.Length != 256)
            {
                throw new ArgumentException(CommonStrings.ShuffleTableMustBe256Bytes, nameof(table));
            }

            if (vector.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, nameof(vector));
            }

            this.table = table.FastClone();
            this.iv = vector.FastClone();
        }

        /// <inheritdoc />
        public byte[] ShuffleIv(byte[] vector)
        {
            Guard.NotNull(() => vector, vector);

            if (vector.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, nameof(vector));
            }

            byte[] shuffled = this.iv.FastClone();

            for (int i = 0; i < 4; i++)
            {
                byte vectorByte = vector[i];

                this.ShuffleIvStep(shuffled, vectorByte);
            }

            return shuffled;
        }

        /// <summary>
        /// Executes a single shuffle step.
        /// </summary>
        /// <param name="shuffled">The array to shuffle.</param>
        /// <param name="vectorByte">The IV input byte.</param>
        protected void ShuffleIvStep(byte[] shuffled, byte vectorByte)
        {
            Guard.NotNull(() => shuffled, shuffled);

            if (shuffled.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, nameof(shuffled));
            }

            byte tableInput = this.table[vectorByte];

            shuffled[0] += (byte)(this.table[shuffled[1]] - vectorByte);
            shuffled[1] -= (byte)(shuffled[2] ^ tableInput);
            shuffled[2] ^= (byte)(this.table[shuffled[3]] + vectorByte);
            shuffled[3] -= (byte)(shuffled[0] - tableInput);

            unchecked
            {
                uint merged = (uint)((shuffled[3] << 24) | (shuffled[2] << 16) | (shuffled[1] << 8) | shuffled[0]);
                uint shifted = (merged << 3) | (merged >> 29);

                shuffled[0] = (byte)shifted;
                shuffled[1] = (byte)(shifted >> 8);
                shuffled[2] = (byte)(shifted >> 16);
                shuffled[3] = (byte)(shifted >> 24);
            }
        }

        /// <inheritdoc />
        public abstract void TransformArraySegment(byte[] data, byte[] vector, int segmentStart, int segmentEnd);

        /// <inheritdoc />
        public byte[] TransformWithIv(byte[] data, byte[] vector)
        {
            Guard.NotNull(() => data, data);
            Guard.NotNull(() => vector, vector);
            
            if (vector.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes);
            }

            var copy = data.FastClone();
            this.TransformArraySegment(copy, vector, 0, copy.Length);
            return copy;
        }
    }
}
