using System;
using OpenStory.Common.Tools;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents an encryption transformer based on the custom KMST algorithm.
    /// </summary>
    public sealed class KmstEncryptor : CryptoTransformBase
    {
        /// <summary>
        /// Initializes a new instance of <see cref="KmstEncryptor"/>.
        /// </summary>
        /// <inheritdoc />
        public KmstEncryptor(byte[] table, byte[] initialIv)
            : base(table, initialIv)
        {
        }

        /// <inheritdoc />
        public override void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd)
        {
            Guard.NotNull(() => data, data);
            Guard.NotNull(() => iv, iv);

            if (iv.Length != 4)
            {
                throw new ArgumentException(CommonStrings.IvMustBe4Bytes, "iv");
            }

            // Thanks to Diamondo25 for this.
            byte[] stepIv = iv.FastClone();
            for (int i = segmentStart; i < segmentEnd; i++)
            {
                byte initial = data[i];

                byte r = (byte)((initial << 4) | (initial >> 4));
                byte a = (byte)((r >> 1) & 0x55);
                byte b = (byte)((r & 0xD5) << 1);
                byte x = (byte)(a | b);

                data[i] = (byte)(this.Table[stepIv[0]] ^ x);

                // NOTE: passing the initial value is CORRECT.
                this.ShuffleIvStep(stepIv, initial);
            }
        }
    }
}
