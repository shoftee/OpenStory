using System;
using OpenStory.Common;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents a decryption transformer based on the custom KMST algorithm.
    /// </summary>
    public sealed class KmstDecryptor : CryptoTransformBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KmstDecryptor"/>.
        /// </summary>
        /// <inheritdoc />
        public KmstDecryptor(byte[] table, byte[] vector)
            : base(table, vector)
        {
        }

        /// <inheritdoc />
        public override void TransformArraySegment(byte[] data, byte[] vector, int segmentStart, int segmentEnd)
        {
            Guard.NotNull(() => data, data);
            Guard.NotNull(() => vector, vector);

            if (vector.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, nameof(vector));
            }

            // Thanks to Diamondo25 for this.
            byte[] stepIv = vector.FastClone();
            for (int i = segmentStart; i < segmentEnd; i++)
            {
                byte initial = data[i];

                byte x = (byte)(initial ^ this.Table[stepIv[0]]);
                byte b = (byte)((x >> 1) & 0x55);
                byte a = (byte)((x & 0xD5) << 1);
                byte r = (byte)(a | b);

                data[i] = (byte)((r >> 4) | (r << 4));

                // NOTE: passing the new value is CORRECT.
                this.ShuffleIvStep(stepIv, data[i]);
            }
        }
    }
}
