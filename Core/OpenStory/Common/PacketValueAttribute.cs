using System;

namespace OpenStory.Common
{
    /// <summary>
    /// Used to decorate <see langword="enum" /> members with their corresponding value in a serialized packet.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class PacketValueAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketValueAttribute"/> class.
        /// </summary>
        /// <param name="value">The packet value to set.</param>
        public PacketValueAttribute(int value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the packet value for this attribute.
        /// </summary>
        public int Value { get; }
    }
}
