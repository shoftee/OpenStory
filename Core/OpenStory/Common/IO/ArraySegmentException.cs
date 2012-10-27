using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// The exception that is thrown when an array 
    /// segment does not fit into an array's bounds.
    /// </summary>
    /// <remarks>
    /// This exception is to be used when an array 
    /// segment defined by either a start and an end offset 
    /// or a start offset and a segment length,
    /// falls outside of an array's bounds.
    /// </remarks>
    [Serializable]
    [Localizable(true)]
    public sealed class ArraySegmentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ArraySegmentException"/>.
        /// </summary>
        /// <inheritdoc />
        private ArraySegmentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Gets a new instance of <see cref="ArraySegmentException"/> with a message that
        /// an array segment with a given start offset and length does not fit into the array's bounds.
        /// </summary>
        /// <param name="startOffset">The start offset of the invalid segment.</param>
        /// <param name="length">The length of the invalid segment.</param>
        /// <returns>an instance of <see cref="ArraySegmentException"/>.</returns>
        public static ArraySegmentException GetByStartAndLength(int startOffset, int length)
        {
            string formatted = String.Format(CultureInfo.CurrentCulture, Exceptions.BadArraySegmentLength, startOffset, length);
            return new ArraySegmentException(formatted);
        }
    }
}
