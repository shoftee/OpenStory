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
    public sealed class PacketReadException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PacketReadException"/>.
        /// </summary>
        /// <inheritdoc />
        private PacketReadException(string message)
            : base(message)
        {
        }

        /// <inheritdoc />
        private PacketReadException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Constructs a <see cref="PacketReadException"/> which states that the end of the stream was reached.
        /// </summary>
        public static PacketReadException EndOfStream()
        {
            return new PacketReadException("The end of the stream was reached.");
        }
    }
}
