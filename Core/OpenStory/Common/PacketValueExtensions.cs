using System;
using System.Linq;

namespace OpenStory.Common
{
    /// <summary>
    /// Extension class for getting <see cref="PacketValueAttribute"/> mappings from <see langword="enum" /> types.
    /// </summary>
    public static class PacketValueExtensions
    {
        /// <summary>
        /// Retrieves the packet value for the provided <see langword="enum" /> member.
        /// </summary>
        /// <param name="enumValue">The <see langword="enum" /> member for which to get the packet value.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="enumValue"/> is not defined as a named constant.</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="enumValue"/> is not decorated with a <see cref="PacketValueAttribute"/>.</exception>
        /// <returns>the integral packet value.</returns>
        public static int ToPacketValue(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);
            if (name == null)
            {
                throw new ArgumentOutOfRangeException("enumValue", enumValue, "The provided value must be a named constant.");
            }

            var field = type.GetField(name);
            var attribute = field.GetCustomAttributes(false).OfType<PacketValueAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                throw new ArgumentException("The provided named constant must be decorated with the PacketValueAttribute attribute.", "enumValue");
            }

            return attribute.Value;
        }
    }
}