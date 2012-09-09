using System;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Encapsulates cryptographic transformation data and routines for the rolling packet encryption and decryption.
    /// </summary>
    public abstract class CryptoTransformBase : ICryptoAlgorithm
    {
        private readonly byte[] table;
        private readonly byte[] initialValue;

        /// <summary>
        /// Gets the translation table used for the IV shuffle.
        /// </summary>
        protected byte[] Table
        {
            get { return table; }
        }

        /// <summary>
        /// Gets the initial IV used for the IV shuffle.
        /// </summary>
        protected byte[] InitialValue
        {
            get { return initialValue; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CryptoTransformBase"/>.
        /// </summary>
        /// <remarks>
        /// The provided arrays are copied into the <see cref="CryptoTransformBase"/> instance to avoid mutation.
        /// </remarks>
        /// <param name="table">The shuffle transformation table.</param>
        /// <param name="initialValue">The initial value for the shuffle transformation.</param>
        /// <exception cref="ArgumentNullException">Thrown if any of the provided parameters is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if any of the provided parameters has an invalid number of elements.</exception>
        protected CryptoTransformBase(byte[] table, byte[] initialValue)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            if (initialValue == null)
            {
                throw new ArgumentNullException("initialValue");
            }

            if (table.Length != 256)
            {
                throw new ArgumentException("'table' must have exactly 256 elements.", "table");
            }
            if (initialValue.Length != 4)
            {
                throw new ArgumentException("'initialValue' must have exactly 4 elements.", "initialValue");
            }

            this.table = table.FastClone();
            this.initialValue = initialValue.FastClone();
        }

        /// <inheritdoc />
        public byte[] ShuffleIv(byte[] iv)
        {
            byte[] shuffled = this.initialValue.FastClone();

            for (int i = 0; i < 4; i++)
            {
                byte ivInput = iv[i];

                ShuffleIvStep(shuffled, ivInput);
            }
            return shuffled;
        }

        /// <summary>
        /// Executes a single shuffle step.
        /// </summary>
        /// <param name="shuffled">The array to shuffle.</param>
        /// <param name="ivInput">The IV input byte.</param>
        protected void ShuffleIvStep(byte[] shuffled, byte ivInput)
        {
            byte tableInput = this.table[ivInput];

            shuffled[0] += (byte)(this.table[shuffled[1]] - ivInput);
            shuffled[1] -= (byte)(shuffled[2] ^ tableInput);
            shuffled[2] ^= (byte)(this.table[shuffled[3]] + ivInput);
            shuffled[3] -= (byte)(shuffled[0] - tableInput);

            unchecked
            {
                uint merged = (uint)((shuffled[3] << 24) | (shuffled[2] << 16) | (shuffled[1] << 8) | shuffled[0]);
                uint shifted = (merged << 3) | (merged >> 29);

                shuffled[0] = (byte)(shifted);
                shuffled[1] = (byte)(shifted >> 8);
                shuffled[2] = (byte)(shifted >> 16);
                shuffled[3] = (byte)(shifted >> 24);
            }
        }

        /// <inheritdoc />
        public abstract void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd);

        /// <inheritdoc />
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
            this.TransformArraySegment(copy, iv, 0, copy.Length);
            return copy;
        }
    }
}
