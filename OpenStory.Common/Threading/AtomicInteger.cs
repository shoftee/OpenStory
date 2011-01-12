using System.Threading;

namespace OpenStory.Common.Threading
{
    /// <summary>
    /// Represents a thread-safe <see cref="T:System.Int32"/> value.
    /// </summary>
    public class AtomicInteger
    {
        private int value;

        /// <summary>
        /// Initializes a new instance of AtomicInteger with the given value.
        /// </summary>
        /// <param name="initialValue">The initial value for the AtomicInteger.</param>
        public AtomicInteger(int initialValue)
        {
            this.value = initialValue;
        }

        /// <summary>
        /// The current value of the AtomicInteger.
        /// </summary>
        public int Value
        {
            get { return this.value; }
        }

        /// <summary>
        /// Increments the value of this AtomicInteger by one and returns the new value.
        /// </summary>
        /// <returns>The value after being incremented.</returns>
        public int Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        /// <summary>
        /// Decrements the value of this AtomicInteger by one and returns the new value.
        /// </summary>
        /// <returns>The value after being decremented.</returns>
        public int Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        /// <summary>
        /// Exchanges the value of this AtomicInteger by with <paramref name="newValue"/> and returns the original value.
        /// </summary>
        /// <param name="newValue">The new value for the AtomicInteger.</param>
        /// <returns>The original value of the AtomicInteger.</returns>
        public int ExchangeWith(int newValue)
        {
            return Interlocked.Exchange(ref this.value, newValue);
        }

        /// <summary>
        /// Compares the value of this AtomicInteger with <paramref name="comparand"/> and assigns <paramref name="newValue"/> if they are equal.
        /// </summary>
        /// <param name="comparand">The value to compare for equality with.</param>
        /// <param name="newValue">The value to assign if the AtomicInteger and comparand are equal.</param>
        /// <returns>The original value of the AtomicInteger.</returns>
        public int CompareExchange(int comparand, int newValue)
        {
            return Interlocked.CompareExchange(ref this.value, newValue, comparand);
        }

        public static implicit operator AtomicInteger(int value)
        {
            return new AtomicInteger(value);
        }

        public static explicit operator int(AtomicInteger atomicInteger)
        {
            return atomicInteger.value;
        }
    }
}