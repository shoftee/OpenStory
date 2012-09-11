using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Thrown when there is a problem with reading a packet.
    /// </summary>
    [Serializable]
    public sealed class PacketReadingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PacketReadingException"/>.
        /// </summary>
        /// <inheritdoc />
        private PacketReadingException(string message)
            : base(message)
        {
        }

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
            return new PacketReadingException("The end of the stream was reached.");
        }
    }
}
