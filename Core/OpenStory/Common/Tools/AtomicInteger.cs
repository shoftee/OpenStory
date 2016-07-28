using System;
using System.Threading;

namespace OpenStory.Common
{
    /// <summary>
    /// Represents a thread-safe <see cref="Int32"/> value.
    /// </summary>
    public sealed class AtomicInteger
    {
        private int value;

        /// <summary>
        /// Gets the current value of the <see cref="AtomicInteger"/>.
        /// </summary>
        public int Value => this.value;

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomicInteger"/> class with the given value.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public AtomicInteger(int initialValue)
        {
            this.value = initialValue;
        }

        /// <summary>
        /// Increments the value of this <see cref="AtomicInteger"/> by one and returns the new value.
        /// </summary>
        /// <returns>the value after being incremented.</returns>
        public int Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        /// <summary>
        /// Decrements the value of this <see cref="AtomicInteger"/> by one and returns the new value.
        /// </summary>
        /// <returns>the value after being decremented.</returns>
        public int Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        /// <summary>
        /// Exchanges the value of this <see cref="AtomicInteger"/> by with <paramref name="newValue"/> and returns the original value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>the original value of the <see cref="AtomicInteger"/>.</returns>
        public int ExchangeWith(int newValue)
        {
            return Interlocked.Exchange(ref this.value, newValue);
        }

        /// <summary>
        /// Assigns a new value to the <see cref="AtomicInteger"/> if the current value is equal to a specified one, and returns the original value.
        /// </summary>
        /// <remarks>
        /// See <see cref="Interlocked.CompareExchange(ref int,int,int)"/> for details.
        /// </remarks>
        /// <param name="comparand">The value to compare for equality with.</param>
        /// <param name="newValue">The value to assign if the <see cref="AtomicInteger"/> and comparand are equal.</param>
        /// <returns>the original value of the <see cref="AtomicInteger"/>.</returns>
        public int CompareExchange(int comparand, int newValue)
        {
            return Interlocked.CompareExchange(ref this.value, newValue, comparand);
        }

        #region Cast methods

        /// <summary>
        /// Creates a new instance of <see cref="AtomicInteger"/> from a provided <see cref="Int32"/>.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>a new instance of <see cref="AtomicInteger"/> with the given initial value.</returns>
        public static implicit operator AtomicInteger(int value)
        {
            return new AtomicInteger(value);
        }

        /// <summary>
        /// Extracts a <see cref="Int32"/> from an instance of <see cref="AtomicInteger"/>.
        /// </summary>
        /// <param name="atomicInteger">The <see cref="AtomicInteger"/> to extract the value of.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="atomicInteger" /> is <see langword="null"/>.
        /// </exception>
        /// <returns>the value of the <see cref="AtomicInteger"/>.</returns>
        public static explicit operator int(AtomicInteger atomicInteger)
        {
            if (atomicInteger == null)
            {
                throw new InvalidCastException();
            }

            return atomicInteger.value;
        }

        /// <summary>
        /// Extracts a <see cref="Int32"/> from an <see cref="AtomicInteger"/> instance.
        /// </summary>
        /// <returns>the value of the <see cref="AtomicInteger"/>.</returns>
        public int ToInt32()
        {
            return this.value;
        }

        /// <summary>
        /// Extracts a <see cref="Int32"/> from an instance of <see cref="AtomicInteger"/>.
        /// </summary>
        /// <param name="atomicInteger">The <see cref="AtomicInteger"/> to extract the value of.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="atomicInteger" /> is <see langword="null"/>.
        /// </exception>
        /// <returns>the value of the <see cref="AtomicInteger"/>.</returns>
        public static int ToInt32(AtomicInteger atomicInteger)
        {
            if (atomicInteger == null)
            {
                throw new InvalidCastException();
            }

            return atomicInteger.value;
        }

        #endregion
    }
}
