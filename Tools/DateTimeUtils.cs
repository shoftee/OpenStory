using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMaple.Tools
{
    static class DateTimeUtils
    {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero);

        /// <summary>
        /// Gets <see cref="DateTimeOffset.UtcNow"/> as Epoch time.
        /// This method is equivalent to Java's System.currentTimeMillis().
        /// </summary>
        /// <returns></returns>
        public static long GetMillisecondsSinceEpoch()
        {
            return (long) (Epoch - DateTimeOffset.UtcNow).TotalMilliseconds;
        }

        /// <summary>
        /// Gets <see cref="DateTimeOffset.UtcNow"/> as FileTime.
        /// </summary>
        /// <returns></returns>
        public static long GetUtcNowAsFileTime()
        {
            return DateTimeOffset.UtcNow.ToFileTime();
        }

        /// <summary>
        /// Returns a FileTime timestamp as a DateTimeOffset.
        /// </summary>
        /// <param name="fileTime">The FileTime timestamp to convert.</param>
        /// <returns>A DateTimeOffset object equivalent to the given timestamp.</returns>
        public static DateTimeOffset GetFileTimeAsUtc(long fileTime)
        {
            return DateTimeOffset.FromFileTime(fileTime);
        }
    }
}
