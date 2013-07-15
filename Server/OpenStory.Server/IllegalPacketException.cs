using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OpenStory.Server
{
    /// <summary>
    /// Represents the error when a packet contains incorrect information.
    /// </summary>
    [Serializable]
    public class IllegalPacketException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPacketException"/> class.
        /// </summary>
        public IllegalPacketException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPacketException"/> class.
        /// </summary>
        public IllegalPacketException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPacketException"/> class.
        /// </summary>
        public IllegalPacketException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalPacketException"/> class.
        /// </summary>
        protected IllegalPacketException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
