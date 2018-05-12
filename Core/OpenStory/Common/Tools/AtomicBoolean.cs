using System;
using System.Threading;

namespace OpenStory.Common
{
    /// <summary>
    /// Represents a thread-safe <see cref="Boolean"/> value.
    /// </summary>
    public sealed class AtomicBoolean
    {
        /// <summary>
        /// Since bleeping <see cref="Interlocked"/> doesn't work with Boolean.
        /// </summary>
        private int _value;

        /// <summary>
        /// Gets the current value of the <see cref="AtomicBoolean"/>.
        /// </summary>
        public bool Value => Convert.ToBoolean(_value);

        /// <summary>
        /// Initializes a new instance of the <see cref="AtomicBoolean"/> class.
        /// </summary>
        /// <param name="initialValue">The initial value.</param>
        public AtomicBoolean(bool initialValue)
        {
            _value = Convert.ToInt32(initialValue);
        }

        /// <summary>
        /// Flips the value of the <see cref="AtomicBoolean"/> if it is equal to the specified boolean value.
        /// </summary>
        /// <param name="comparand">The value to compare with.</param>
        /// <returns><see langword="true" /> if the flip took place; otherwise, <see langword="false" />.</returns>
        public bool FlipIf(bool comparand)
        {
            int comparandAsInt = Convert.ToInt32(comparand);
            int newValueAsInt = Convert.ToInt32(!comparand);
            int originalValueAsInt = Interlocked.CompareExchange(ref _value, newValueAsInt, comparandAsInt);

            return originalValueAsInt == comparandAsInt;
        }

        /// <summary>
        /// Sets the value of the <see cref="AtomicBoolean"/> to the provided value.
        /// </summary>
        /// <param name="newValue">The new value to assign.</param>
        public void Set(bool newValue)
        {
            int newValueAsInt = Convert.ToInt32(newValue);
            Interlocked.Exchange(ref _value, newValueAsInt);
        }

        #region Cast methods

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
        /// Extracts a <see cref="Boolean"/> from an instance of <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="atomicBoolean">The <see cref="AtomicBoolean"/> to extract the value of.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="atomicBoolean" /> is <see langword="null"/>.
        /// </exception>
        /// <returns>the value of the <see cref="AtomicBoolean"/>.</returns>
        public static explicit operator bool(AtomicBoolean atomicBoolean)
        {
            if (atomicBoolean == null)
            {
                throw new InvalidCastException();
            }

            return atomicBoolean.Value;
        }

        /// <summary>
        /// Extracts a <see cref="Boolean"/> from an <see cref="AtomicBoolean"/> instance.
        /// </summary>
        /// <returns>the value of the <see cref="AtomicBoolean"/>.</returns>
        public bool ToBoolean()
        {
            return Value;
        }

        /// <summary>
        /// Extracts a <see cref="Boolean"/> from an instance of <see cref="AtomicBoolean"/>.
        /// </summary>
        /// <param name="atomicBoolean">The <see cref="AtomicBoolean"/> to extract the value of.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="atomicBoolean" /> is <see langword="null"/>.
        /// </exception>
        /// <returns>the value of the <see cref="AtomicBoolean"/>.</returns>
        public static bool ToBoolean(AtomicBoolean atomicBoolean)
        {
            if (atomicBoolean == null)
            {
                throw new InvalidCastException();
            }

            return atomicBoolean.Value;
        }

        #endregion
    }
}
