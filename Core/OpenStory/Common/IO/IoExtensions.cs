using System;
using OpenStory.Common.Game;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Extension methods for packet I/O.
    /// </summary>
    public static class IoExtensions
    {
        // You may use these as samples for writing custom readers/writers. 
        // Just write an extension method to PacketReader or PacketBuilder (or one of their interfaces like below) and it works! Magic!

        /// <summary>
        /// Writes a <see cref="ByteBuffer"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IPacketBuilder">packet builder</see> to use.</param>
        /// <param name="buffer">The <see cref="ByteBuffer"/> to write.</param>
        public static void WriteBuffer(this IPacketBuilder builder, ByteBuffer buffer)
        {
            buffer.Write(builder);
        }

        /// <summary>
        /// Reads a <see cref="PointS"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IUnsafePacketReader">packet reader</see> to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="reader"/> is <see langword="null"/>.</exception>
        /// <returns>a <see cref="PointS"/> that was read.</returns>
        public static PointS ReadVector(this IUnsafePacketReader reader)
        {
            Guard.NotNull(() => reader, reader);

            var x = reader.ReadInt16();
            var y = reader.ReadInt16();
            return new PointS(x, y);
        }

        /// <summary>
        /// Writes a <see cref="PointS"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IPacketBuilder">packet builder</see> to use.</param>
        /// <param name="vector">The <see cref="PointS"/> to write.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> is <see langword="null"/>.</exception>
        /// <inheritdoc cref="PacketBuilder.WriteInt16(short)" select="exception[@cref='ObjectDisposedException']" />
        public static void WriteVector(this IPacketBuilder builder, PointS vector)
        {
            Guard.NotNull(() => builder, builder);

            builder.WriteInt16(vector.X);
            builder.WriteInt16(vector.Y);
        }

        /// <summary>
        /// Reads a flags instance.
        /// </summary>
        /// <typeparam name="TFlags">The <see cref="Flags"/>-derived type to read into.</typeparam>
        /// <param name="reader">The reader to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="reader"/> is <see langword="null"/>.</exception>
        public static TFlags ReadFlags<TFlags>(this IUnsafePacketReader reader)
            where TFlags : Flags, new()
        {
            Guard.NotNull(() => reader, reader);

            var flags = new TFlags();
            flags.Read(reader);
            return flags;
        }

        /// <summary>
        /// Writes a flags instance.
        /// </summary>
        /// <typeparam name="TFlags">The <see cref="Flags"/>-derived type to read into.</typeparam>
        /// <param name="builder">The <see cref="IPacketBuilder">packet builder</see> to use.</param>
        /// <param name="flags">The flags to write.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="builder"/> or <paramref name="flags"/> is <see langword="null"/>.</exception>
        public static void WriteFlags<TFlags>(this IPacketBuilder builder, TFlags flags)
            where TFlags : Flags
        {
            Guard.NotNull(() => builder, builder);
            Guard.NotNull(() => flags, flags);

            flags.Write(builder);
        }

        /// <summary>
        /// Writes a FILETIME timestamp.
        /// </summary>
        /// <param name="builder">The <see cref="IPacketBuilder">packet builder</see> to use.</param>
        /// <param name="instant">An <see cref="DateTimeOffset"/> value.</param>
        /// <inheritdoc cref="PacketBuilder.WriteInt64(long)" select="exception[@cref='ObjectDisposedException']" />
        public static void WriteTimestamp(this IPacketBuilder builder, DateTimeOffset instant)
        {
            Guard.NotNull(() => builder, builder);
            
            var timestamp = instant.ToFileTime();

            builder.WriteInt64(timestamp);
        }

        /// <summary>
        /// Reads a byte corresponding to a <typeparamref name="TEnum" /> value decorated with <see cref="PacketValueAttribute"/>.
        /// </summary>
        /// <typeparam name="TEnum">The target <see langword="enum" /> type.</typeparam>
        /// <param name="reader">The reader to use.</param>
        /// <inheritdoc cref="PacketReader.ReadByte()" select="exception[@cref='PacketReadingException']" />
        /// <returns>the corresponding <typeparamref name="TEnum" /> value.</returns>
        public static TEnum ReadByte<TEnum>(this IUnsafePacketReader reader)
            where TEnum : struct
        {
            Guard.NotNull(() => reader, reader);
            
            return reader.ReadByte().ToEnumValue<TEnum>();
        }

        /// <summary>
        /// Writes a byte from the <see cref="PacketValueAttribute" /> value of the specified enum value.
        /// </summary>
        /// <param name="builder">The <see cref="IPacketBuilder">packet builder</see> to use.</param>
        /// <param name="enumValue">An enum value decorated with <see cref="PacketValueAttribute" />.</param>
        /// <inheritdoc cref="PacketValueExtensions.ToPacketValue(Enum)" select="exception[@cref='ArgumentException']" />
        /// <inheritdoc cref="PacketValueExtensions.ToPacketValue(Enum)" select="exception[@cref='ArgumentOutOfRangeException']" />
        /// <inheritdoc cref="PacketBuilder.WriteByte(int)" select="exception[@cref='ObjectDisposedException']" />
        public static void WriteByte(this IPacketBuilder builder, Enum enumValue)
        {
            Guard.NotNull(() => builder, builder);
            
            builder.WriteByte(enumValue.ToPacketValue());
        }

        /// <summary>
        /// Writes an <see cref="Int32" /> from the <see cref="PacketValueAttribute" /> value of the specified enum value.
        /// </summary>
        /// <param name="builder">The <see cref="IPacketBuilder">packet builder</see> to use.</param>
        /// <param name="enumValue">An enum value decorated with <see cref="PacketValueAttribute" />.</param>
        /// <inheritdoc cref="PacketValueExtensions.ToPacketValue(Enum)" select="exception[@cref='ArgumentException']" />
        /// <inheritdoc cref="PacketValueExtensions.ToPacketValue(Enum)" select="exception[@cref='ArgumentOutOfRangeException']" />
        /// <inheritdoc cref="PacketBuilder.WriteInt32(int)" select="exception[@cref='ObjectDisposedException']" />
        public static void WriteInt32(this IPacketBuilder builder, Enum enumValue)
        {
            Guard.NotNull(() => builder, builder);
            
            builder.WriteInt32(enumValue.ToPacketValue());
        }
    }
}
