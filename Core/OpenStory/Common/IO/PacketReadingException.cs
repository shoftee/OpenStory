using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Thrown when there is a problem with reading a packet.
    /// </summary>
    [Serializable]
    [Localizable(true)]
    public sealed class PacketReadingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReadingException"/> class.
        /// </summary>
        /// <inheritdoc />
        private PacketReadingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReadingException"/> class.
        /// </summary>
        /// <inheritdoc />
        private PacketReadingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Constructs a <see cref="PacketReadingException"/> which states that the end of the stream was reached.
        /// </summary>
        public static PacketReadingException EndOfStream()
        {
            return new PacketReadingException(Exceptions.EndOfStreamReached);
        }
    }
}
