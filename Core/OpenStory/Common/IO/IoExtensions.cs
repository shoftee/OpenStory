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
    }
}
