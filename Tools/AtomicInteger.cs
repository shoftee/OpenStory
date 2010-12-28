using System.Threading;

namespace OpenMaple.Tools
{
    /// <summary>
    /// Represents a thread-safe <see cref="int"/> value.
    /// </summary>
    public struct AtomicInteger
    {
        private int value;

        /// <summary>
        /// The current value of this AtomicInteger instance.
        /// </summary>
        public int Value { get { return this.value; } }

        /// <summary>
        /// Initializes a new instance of AtomicInteger with the given value.
        /// </summary>
        /// <param name="initialValue">The initial value for the AtomicInteger.</param>
        public AtomicInteger(int initialValue)
        {
            this.value = initialValue;
        }

        /// <summary>
        /// Adds <paramref name="increment"/> to this AtomicInteger's value as an atomic operation, and replaces it with the result.
        /// </summary>
        /// <param name="increment">The value to add to the AtomicInteger.</param>
        /// <returns>The new value of the AtomicInteger.</returns>
        public int Add(int increment)
        {
            return Interlocked.Add(ref this.value, increment);
        }

        /// <summary>
        /// Increments the value of this AtomicInteger by one as an atomic operation and returns the new value.
        /// </summary>
        /// <returns>The value after being incremented.</returns>
        public int Increment()
        {
            return Interlocked.Increment(ref this.value);
        }

        /// <summary>
        /// Decrements the value of this AtomicInteger by one as an atomic operation, and returns the new value.
        /// </summary>
        /// <returns>The value after being decremented.</returns>
        public int Decrement()
        {
            return Interlocked.Decrement(ref this.value);
        }

        /// <summary>
        /// Exchanges the value of this AtomicInteger by with <paramref name="newValue"/> as an atomic operation, and returns the original value.
        /// </summary>
        /// <param name="newValue">The new value for the AtomicInteger.</param>
        /// <returns>The original value of the AtomicInteger.</returns>
        public int ExchangeWith(int newValue)
        {
            return Interlocked.Exchange(ref this.value, newValue);
        }

        /// <summary>
        /// Compares the value of this AtomicInteger with <paramref name="comparand"/> and assigns <paramref name="newValue"/> if they are equal, as an atomic operation.
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