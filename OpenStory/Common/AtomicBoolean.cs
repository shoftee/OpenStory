using System;
using System.Threading;

namespace OpenStory.Common
{
    /// <summary>
    /// Represents a thread-safe <see cref="T:System.Boolean"/> value.
    /// </summary>
    public sealed class AtomicBoolean
    {
        /// <summary>
        /// Since bleeping Interlocked doesn't work with Boolean.
        /// </summary>
        private int value;

        /// <summary>
        /// Initializes a new instance of <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public AtomicBoolean(bool initialValue)
        {
            this.value = Convert.ToInt32(initialValue);
        }

        /// <summary>
        /// Gets the current value of the AtomicBoolean.
        /// </summary>
        public bool Value
        {
            get { return Convert.ToBoolean(this.value); }
        }

        /// <summary>
        /// Assigns a new value to the <see cref="AtomicBoolean"/> and returns the original one.
        /// </summary>
        /// <param name="newValue">The value to assign.</param>
        /// <returns>the original value.</returns>
        public bool Exchange(bool newValue)
        {
            int newValueAsInt = Convert.ToInt32(newValue);
            int result = Interlocked.Exchange(ref this.value, newValueAsInt);
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Assigns a new value to the <see cref="AtomicBoolean"/> if the current value is equal to a specified one, and returns the original value.
        /// </summary>
        /// <param name="comparand">The value to compare for equality with.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>the original value.</returns>
        public bool CompareExchange(bool comparand, bool newValue)
        {
            int newValueAsInt = Convert.ToInt32(newValue);
            int comparandAsInt = Convert.ToInt32(comparand);
            int result = Interlocked.CompareExchange(ref this.value, newValueAsInt, comparandAsInt);
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Creates a new instance of <see cref="AtomicBoolean"/> from a provided <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <returns>a new instance of <see cref="AtomicBoolean"/> with the given initial value.</returns>
        public static implicit operator AtomicBoolean(bool value)
        {
            return new AtomicBoolean(value);
        }

        /// <summary>
        /// Extracts a <see cref="System.Boolean"/> from an instance of <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="atomicBoolean">The <see cref="AtomicBoolean"/> to extract the value of.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="atomicBoolean" /> is <c>null</c>.
        /// </exception>
        /// <returns>the value of the <see cref="AtomicBoolean"/>.</returns>
        public static explicit operator bool(AtomicBoolean atomicBoolean)
        {
            if (atomicBoolean == null)
            {
                throw new ArgumentNullException("atomicBoolean");
            }
            return atomicBoolean.Value;
        }

        /// <summary>
        /// Extracts a <see cref="System.Boolean"/> from an <see cref="AtomicBoolean"/> instance.
        /// </summary>
        /// <returns>the value of the <see cref="AtomicBoolean"/>.</returns>
        public bool ToBoolean()
        {
            return this.Value;
        }

        /// <summary>
        /// Extracts a <see cref="System.Boolean"/> from an instance of <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="atomicBoolean">The <see cref="AtomicBoolean"/> to extract the value of.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="atomicBoolean" /> is <c>null</c>.
        /// </exception>
        /// <returns>the value of the <see cref="AtomicBoolean"/>.</returns>
        public static bool ToBoolean(AtomicBoolean atomicBoolean)
        {
            if (atomicBoolean == null)
            {
                throw new ArgumentNullException("atomicBoolean");
            }
            return atomicBoolean.Value;
        }
    }
}
