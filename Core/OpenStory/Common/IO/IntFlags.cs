using System;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// An abstract class for bit-based flag arrays which are written in 32-bit chunks.
    /// </summary>
    public abstract class IntFlags : Flags
    {
        private const int IntBitCount = 32;

        /// <inheritdoc />
        protected IntFlags(int capacity) : base(capacity) { }

        /// <inheritdoc />
        protected IntFlags(IntFlags other) : base(other) { }

        /// <inheritdoc />
        public sealed override void Write(IPacketBuilder builder)
        {
            // TODO: Actually figure out if this is how they're packed.

            int bitCount = base.Bits.Length;
            int numberCount = bitCount / IntBitCount;
            var numbers = new uint[numberCount];

            int numberIndex = 0;
            for (int i = 0; i < bitCount; i++)
            {
                if (i > 0 && i % IntBitCount == 0)
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
                builder.WriteInt32(numbers[i]);
            }
        }

        /// <inheritdoc />
        public sealed override void Read(IUnsafePacketReader reader)
        {
            int bitCount = base.Bits.Length;
            int numberCount = bitCount / IntBitCount;
            
            for (int i = 0; i < numberCount; i++)
            {
                uint number = reader.ReadUInt32();
                int startIndex = i * IntBitCount;
                int endIndex = Math.Min(startIndex + IntBitCount, bitCount);
                for(int j = startIndex; j < endIndex; j ++)
                {
                    base.Bits[j] = Convert.ToBoolean(number & 1);
                    number >>= 1;
                }
            }
        }
    }
}