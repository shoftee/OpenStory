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
        public KmstDecryptor(byte[] table, byte[] initialValue)
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

                byte x = (byte)(initial ^ base.Table[stepIv[0]]);
                byte b = (byte)((x >> 1) & 0x55);
                byte a = (byte)((x & 0xD5) << 1);
                byte r = (byte)(a | b);

                data[i] = (byte)((r >> 4) | (r << 4));

                // NOTE: passing the new value is CORRECT.
                base.ShuffleIvStep(stepIv, data[i]);
            }
        }
    }
}