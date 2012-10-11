using System.Collections;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// An abstract class for bit-based flag arrays.
    /// </summary>
    public abstract class Flags
    {
        /// <summary>
        /// Gets the bit array for this instance.
        /// </summary>
        protected BitArray Bits { get; private set; }

        /// <summary>
        /// Gets or sets the flag value at an index.
        /// </summary>
        /// <param name="index">The index of the flag.</param>
        /// <returns>the value of the flag.</returns>
        protected bool this[int index]
        {
            get { return this.Bits[index]; }
            set { this.Bits[index] = value; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Bits"/>.
        /// </summary>
        /// <param name="capacity">The length of the bit array.</param>
        protected Flags(int capacity)
        {
            this.Bits = new BitArray(capacity);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Bits"/>.
        /// </summary>
        /// <param name="other">The instance to copy from.</param>
        protected Flags(Flags other)
        {
            this.Bits = new BitArray(other.Bits);
        }

        /// <summary>
        /// Writes the flag bits into the specified <see cref="IPacketBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder to write the bits into.</param>
        public abstract void Write(IPacketBuilder builder);

        /// <summary>
        /// Reads the flag bits from the specified <see cref="IUnsafePacketReader"/>.
        /// </summary>
        /// <param name="reader">The reader to use.</param>
        public abstract void Read(IUnsafePacketReader reader);
    }
}