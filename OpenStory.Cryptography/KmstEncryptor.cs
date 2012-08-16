using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Cryptography
{
    /// <summary>
    /// Represents an encryption transformer based on the custom KMST algorithm.
    /// </summary>
    public sealed class KmstEncryptor : CryptoTransformBase
    {
        /// <inheritdoc />
        public KmstEncryptor(byte[] table, byte[] initialValue)
            : base(table, initialValue)
        {
        }

        /// <inheritdoc />
        public override void TransformArraySegment(byte[] data, byte[] iv, int segmentStart, int segmentEnd)
        {
            // Thanks to Diamondo25 for this.
            byte[] stepIv = iv.FastClone();
            for (int i = segmentStart; i < segmentEnd; i++)
            {
                byte initial = data[i];

                byte r = (byte)((initial << 4) | (initial >> 4));
                byte a = (byte)((r >> 1) & 0x55);
                byte b = (byte)((r & 0xD5) << 1);
                byte x = (byte)(a | b);

                data[i] = (byte)(base.Table[stepIv[0]] ^ x);

                // NOTE: passing the initial value is CORRECT.
                base.ShuffleIvStep(stepIv, initial);
            }
        }
    }
}
