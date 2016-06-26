using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using R = OpenStory.CommonStrings;

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
        /// <returns>the numeric packet value.</returns>
        public static int ToPacketValue(this Enum enumValue)
        {
            var type = enumValue.GetType();
            if (!Enum.IsDefined(type, enumValue))
            {
                throw new ArgumentOutOfRangeException(nameof(enumValue), enumValue, R.EnumValueMustBeNamedMember);
            }

            var cache = EnsureCache(type).EnumToNumeric;

            int packetValue;
            if (cache.TryGetValue(enumValue, out packetValue))
            {
                return packetValue;
            }
            else
            {
                var message = string.Format(R.EnumMember_0_MustBeDecoratedWithPacketValue, enumValue);
                throw new ArgumentException(message, nameof(enumValue));
            }
        }

        /// <summary>
        /// Retrieves the <typeparamref name="TEnum" /> value corresponding to the provided numeric packet value.
        /// </summary>
        /// <typeparam name="TEnum">The exact <see langword="enum" /> type.</typeparam>
        /// <param name="packetValue">The numeric value to match to an enum member.</param>
        /// <exception cref="ArgumentException">Thrown if <typeparamref name="TEnum" /> is not an <see langword="enum" /> type.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="packetValue" /> does not match any named member of <typeparamref name="TEnum" />.</exception>
        /// <returns>a <typeparamref name="TEnum" /> value that was found.</returns>
        public static TEnum ToEnumValue<TEnum>(this int packetValue)
            where TEnum : struct
        {
            var type = typeof(TEnum);
            if (!type.IsEnum)
            {
                var message = string.Format(R.TypeArgument_0_MustBeEnum, type.FullName);
                throw new ArgumentException(message);
            }

            object enumValue;
            var cache = EnsureCache(type).NumericToEnum;
            if (cache.TryGetValue(packetValue, out enumValue))
            {
                return (TEnum) enumValue;
            }
            else
            {
                var message = string.Format(R.PacketValueDoesNotMatchAnyMemberOfType_0, type.FullName);
                throw new ArgumentOutOfRangeException(nameof(packetValue), packetValue, message);
            }
        }

        /// <inheritdoc cref="ToEnumValue{TEnum}(int)" />
        public static TEnum ToEnumValue<TEnum>(this byte packetValue)
            where TEnum : struct
        {
            return ToEnumValue<TEnum>((int)packetValue);
        }

        private static readonly ConcurrentDictionary<Type, PacketValueCache> PacketValueCaches = new ConcurrentDictionary<Type, PacketValueCache>();

        private static PacketValueCache EnsureCache(Type type)
        {
            var cache = PacketValueCaches.GetOrAdd(type, t => new PacketValueCache(t));
            return cache;
        }

        private sealed class PacketValueCache
        {
            private const BindingFlags PublicStaticFields = BindingFlags.Public | BindingFlags.Static;

            public Dictionary<object, int> EnumToNumeric { get; }

            public Dictionary<int, object> NumericToEnum { get; }

            public PacketValueCache(Type enumType)
            {
                var mappings =
                    (from field in enumType.GetFields(PublicStaticFields)
                     from attribute in field.GetCustomAttributes(typeof(PacketValueAttribute), false)
                     where attribute != null
                     select new
                            {
                                EnumValue = field.GetValue(null),
                                PacketValue = ((PacketValueAttribute)attribute).Value
                            })
                        .ToList();

                this.EnumToNumeric = mappings.ToDictionary(c => c.EnumValue, c => c.PacketValue);
                this.NumericToEnum = mappings.ToDictionary(c => c.PacketValue, c => c.EnumValue);
            }
        }
    }
}