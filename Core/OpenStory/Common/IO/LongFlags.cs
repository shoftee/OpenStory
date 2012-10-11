using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// An abstract class for bit-based flag arrays which are written in 64-bit chunks.
    /// </summary>
    public abstract class LongFlags : Flags
    {
        private const int LongBitCount = 64;

        /// <inheritdoc />
        protected LongFlags(int capacity) : base(capacity) { }

        /// <inheritdoc />
        protected LongFlags(LongFlags other) : base(other) { }

        /// <inheritdoc />
        public sealed override void Write(IPacketBuilder builder)
        {
            // TODO: Actually figure out if this is how they're packed.

            var bitCount = base.Bits.Length;
            var numberCount = bitCount / LongBitCount;
            var numbers = new ulong[numberCount];

            int numberIndex = 0;
            for (int i = 0; i < bitCount; i++)
            {
                if (i > 0 && i % LongBitCount == 0)
                {
                    numberIndex++;
                }
                else
                {
                    numbers[numberIndex] <<= 1;
                }
                numbers[numberIndex] |= Convert.ToUInt32(base.Bits[i]);
            }

            for (int i = 0; i < numberCount; i++)
            {
                builder.WriteInt64(numbers[i]);
            }
        }
        
        /// <inheritdoc />
        public sealed override void Read(IUnsafePacketReader reader)
        {
            int bitCount = base.Bits.Length;
            int numberCount = bitCount / LongBitCount;

            for (int i = 0; i < numberCount; i++)
            {
                ulong number = reader.ReadUInt64();
                int startIndex = i * LongBitCount;
                int endIndex = Math.Min(startIndex + LongBitCount, bitCount);
                for (int j = startIndex; j < endIndex; j++)
                {
                    base.Bits[j] = Convert.ToBoolean(number & 1);
                    number >>= 1;
                }
            }
        }
    }
}