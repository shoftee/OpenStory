using System;
using System.Threading;

namespace OpenMaple.Threading
{
    /// <summary>
    /// Represents a thread-safe <see cref="T:System.Boolean"/> value.
    /// </summary>
    public class AtomicBoolean
    {
        /// <summary>
        /// Since bleeping Interlocked doesn't work with Boolean.
        /// </summary>
        private int value;

        /// <summary>
        /// Gets the current value of the AtomicBoolean.
        /// </summary>
        public bool Value { get { return Convert.ToBoolean(this.value); } }

        /// <summary>
        /// Initializes a new instance of AtomicBoolean.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public AtomicBoolean(bool initialValue)
        {
            this.value = Convert.ToInt32(initialValue);
        }

        /// <summary>
        /// Assigns a new value to the AtomicBoolean and returns the original one.
        /// </summary>
        /// <param name="newValue">The value to assign.</param>
        /// <returns>The original value.</returns>
        public bool Exchange(bool newValue)
        {
            int newValueAsInt = Convert.ToInt32(newValue);
            int result = Interlocked.Exchange(ref this.value, newValueAsInt);
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Assigns <paramref name="newValue"/> to the AtomicBoolean if the current value and <paramref name="comparand"/> are equal, and returns the original value.
        /// </summary>
        /// <param name="comparand">The value to compare for equality with.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>The original value of the AtomicBoolean.</returns>
        public bool CompareExchange(bool comparand, bool newValue)
        {
            int newValueAsInt = Convert.ToInt32(newValue);
            int comparandAsInt = Convert.ToInt32(comparand);
            int result = Interlocked.CompareExchange(ref this.value, newValueAsInt, comparandAsInt);
            return Convert.ToBoolean(result);
        }
    }
}