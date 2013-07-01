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
        protected LongFlags(int capacity)
            : base(capacity)
        {
        }

        /// <inheritdoc />
        protected LongFlags(LongFlags other)
            : base(other)
        {
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <c>null</c>.</exception>
        public sealed override void Write(IPacketBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException("builder");
            }

            // TODO: Actually figure out if this is how they're packed.

            var bitCount = this.Bits.Length;
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

                numbers[numberIndex] |= Convert.ToUInt32(this.Bits[i]);
            }

            for (int i = 0; i < numberCount; i++)
            {
                builder.WriteInt64(numbers[i]);
            }
        }

        /// <exception cref="ArgumentNullException">Thrown if <paramref name="reader"/> is <c>null</c>.</exception>
        /// <inheritdoc />
        public sealed override void Read(IUnsafePacketReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            int bitCount = this.Bits.Length;
            int numberCount = bitCount / LongBitCount;

            for (int i = 0; i < numberCount; i++)
            {
                ulong number = reader.ReadUInt64();
                int startIndex = i * LongBitCount;
                int endIndex = Math.Min(startIndex + LongBitCount, bitCount);
                for (int j = startIndex; j < endIndex; j++)
                {
                    this.Bits[j] = Convert.ToBoolean(number & 1);
                    number >>= 1;
                }
            }
        }
    }
}