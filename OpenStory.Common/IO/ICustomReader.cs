using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenStory.Common.IO
{
    /// <summary>
    /// Provides methods for reading custom objects from a <see cref="PacketReader"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects that are going to be read.</typeparam>
    public interface ICustomReader<T>
    {
        /// <summary>
        /// Reads an object using a <see cref="IUnsafePacketReader"/>.
        /// </summary>
        /// <param name="reader">The instance of <see cref="IUnsafePacketReader"/> to use.</param>
        /// <returns>the object that was read.</returns>
        T Read(IUnsafePacketReader reader);

        /// <summary>
        /// Attempts to read an object using a <see cref="ISafePacketReader"/>.
        /// </summary>
        /// <param name="reader">The instance of <see cref="ISafePacketReader"/> to use.</param>
        /// <param name="value">A variable to hold the result.</param>
        /// <returns><c>true</c> if the object was read successfully; otherwise, <c>false</c>.</returns>
        bool TryRead(ISafePacketReader reader, out T value);
    }
}
