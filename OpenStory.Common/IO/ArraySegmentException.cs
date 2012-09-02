using System;
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
    /// Please provide meaningful messages or 
    /// use the static methods provided.
    /// </remarks>
    [Serializable]
    public sealed class ArraySegmentException : ArgumentException
    {
        private ArraySegmentException(string message)
            : base(message)
        {
        }

        private ArraySegmentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
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
            const string LengthFormat =
                "The array segment starting at {0} with length {1} does not fit into the array's bounds.";

            string formatted = String.Format(CultureInfo.InvariantCulture, LengthFormat, startOffset, length);
            return new ArraySegmentException(formatted);
        }

        /// <summary>
        /// Gets a new instance of <see cref="ArraySegmentException"/> with a message that
        /// an array segment with given start and end offsets does not fit into the array's bounds.
        /// </summary>
        /// <param name="startOffset">The start offset of the invalid segment.</param>
        /// <param name="endOffset">The end offset of the invalid segment.</param>
        /// <returns>an instance of <see cref="ArraySegmentException"/>.</returns>
        public static ArraySegmentException GetByStartAndEnd(int startOffset, int endOffset)
        {
            const string BoundsFormat =
                "The array segment [{0},{1}] does not fit into the array's bounds.";

            string formatted = String.Format(CultureInfo.InvariantCulture, BoundsFormat, startOffset, endOffset);
            return new ArraySegmentException(formatted);
        }
    }
}
