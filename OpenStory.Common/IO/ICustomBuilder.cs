using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for writing custom objects to a <see cref="PacketBuilder"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects that are going to be written.</typeparam>
    public interface ICustomBuilder<T>
    {
        /// <summary>
        /// Writes a custom object using the provided <see cref="IPacketBuilder"/>.
        /// </summary>
        /// <param name="builder">The builder to use.</param>
        /// <param name="obj">The object to write.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="builder"/> is <c>null</c>.
        /// </exception>
        void WriteCustom(IPacketBuilder builder, T obj);
    }
}
